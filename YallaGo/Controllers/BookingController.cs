using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using System.Text;
using YallaGo.DAL.Repositories;
using System.Security.Claims;
using YallaGo.DAL.Consts;
using YallaGo.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace YallaGo.UI.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private string PayPalClientId { get; set; } = "";
        private string PayPalSecret { get; set; } = "";
        private string PayPalUrl { get; set; } = "";

        private readonly IUnitOfWork _unitOfWork;
        public BookingController(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            PayPalClientId = configuration["PayPal:ClientId"];
            PayPalSecret = configuration["PayPal:Secret"];
            PayPalUrl = configuration["PayPal:Url"];
            _unitOfWork = unitOfWork;
        }
        //public async Task<string> Token()
        //{
        //    string accessToken = await GetPayPalAccessToken();
        //    return accessToken;
        //}
        private async Task<string> GetPayPalAccessToken()
        {
            string accessToken = "";

            string url = PayPalUrl + "/v1/oauth2/token";

            using (HttpClient client = new HttpClient())
            {
                string credentials64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(PayPalClientId + ":" + PayPalSecret));

                client.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials64);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("grant_type=client_credentials", null, "application/x-www-form-urlencoded");

                var httpResponse = await client.SendAsync(requestMessage);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();

                    var jsonResponse = JsonNode.Parse(strResponse);
                    if (jsonResponse != null)
                    {
                        accessToken = jsonResponse["access_token"]?.ToString() ?? "";
                    }

                }
            }
            return accessToken;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] JsonObject data)
        {
            var amount = data?["amount"]?.ToString();

            if (string.IsNullOrEmpty(amount))
                return BadRequest("Amount is required.");

            string accessToken = await GetPayPalAccessToken();

            var orderRequest = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
            new {
                amount = new {
                    currency_code = "USD",
                    value = amount
                }
            }
        }
            };

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(orderRequest), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(PayPalUrl + "/v2/checkout/orders", content);

                if (!response.IsSuccessStatusCode)
                    return BadRequest("Failed to create PayPal order.");

                var responseBody = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonNode.Parse(responseBody);

                string orderId = jsonResponse?["id"]?.ToString();
                return Json(new { id = orderId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CompleteOrder([FromBody] JsonObject data)
        {

            var orderId = data?["orderID"]?.ToString();
            if (orderId == null)
            {
                return new JsonResult("error");
            }

            string accessToken = await GetPayPalAccessToken();
            string url = PayPalUrl + "/v2/checkout/orders/" + orderId + "/capture";


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("", null, "application/json");
                var httpResponse = await client.SendAsync(requestMessage);



                if (httpResponse.IsSuccessStatusCode)
                {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);
                    if (jsonResponse != null)
                    {
                        string paypalOrderStatus = jsonResponse["status"]?.ToString() ?? "";
                        if (paypalOrderStatus == "COMPLETED")
                        {

                            var booking = new Booking();
                            booking.NumberOfPeople = int.Parse(data["numberOfPeople"].ToString());
                            booking.TotalPrice = decimal.Parse(data["amount"].ToString());
                            booking.Status = BookingStatus.Completed;
                            booking.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                            booking.TourId = int.Parse(data["tourId"].ToString());
                            booking.BookingDate = DateTime.Now;
                            await _unitOfWork.BookingRepo.AddAsync(booking);
                            await _unitOfWork.CompleteAsync();

                            return new JsonResult("success");
                        }


                    }
                }

            }
            var cancelledbooking = new Booking();
            cancelledbooking.NumberOfPeople = int.Parse(data["numberOfPeople"].ToString());
            cancelledbooking.TotalPrice = decimal.Parse(data["amount"].ToString());
            cancelledbooking.Status = BookingStatus.Cancelled;
            cancelledbooking.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            cancelledbooking.TourId = int.Parse(data["tourId"].ToString());
            cancelledbooking.BookingDate = DateTime.Now;
            await _unitOfWork.BookingRepo.AddAsync(cancelledbooking);
            await _unitOfWork.CompleteAsync();

            return new JsonResult("error");

        }
        public async Task<IActionResult> Index(int id)
        {
            var tour = await _unitOfWork.TourRepo.GetByIdAsync(id);
            if (tour == null)
            {
                return NotFound();
            }
            ViewBag.Price = tour.Price;
            ViewBag.TourId = id;
            ViewBag.ClientId = PayPalClientId;
            return View();
        }

        [Authorize(Roles = "Admin,owner")]
        public async Task<IActionResult> BookingHistory()
        {
            var bookings = await _unitOfWork.BookingRepo.GetAllAsync();
            return View(bookings);
        }

        public async Task<IActionResult> History()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var bookings = await _unitOfWork.BookingRepo.GetAllAsync(b => b.UserId == userId);
            return View(bookings);
        }

        public async Task<IActionResult> Details(int id)
        {
            var booking = await _unitOfWork.BookingRepo.FindAsync(b => b.Id == id, new[] {"Tour"});
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _unitOfWork.BookingRepo.GetByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            _unitOfWork.BookingRepo.HardDelete(booking);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction("BookingHistory");

        }


    }
}
