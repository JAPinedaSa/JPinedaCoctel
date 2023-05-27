using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class CoctelController : Controller
    {
        [HttpGet]
        public IActionResult GetByIdLetra(string Letra)
        {
            ML.Result result = new ML.Result();
            ML.Coctel coctel = new ML.Coctel();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("www.thecocktaildb.com/api/");
                var responseTask = client.GetAsync("json/v1/1/search.php/" + Letra);
                responseTask.Wait();
                var resultAPI = responseTask.Result;
                if (resultAPI.IsSuccessStatusCode)
                {
                    var readTask = resultAPI.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();
                    ML.Coctel resultItemList = new ML.Coctel();
                    resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Coctel>(readTask.Result.Object.ToString());
                    result.Object = resultItemList;


                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No existen registros en la tabla Libro";
                }

            }
            if (result.Correct)
            {

                coctel = (ML.Coctel)result.Object;



            
                return View(coctel);
            }
            else
            {
                ViewBag.Message = "Ocurrio un error al hacer la consulta:" + result.ErrorMessage;
                return View("Modal");
            }
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            //Letra = "a";
            ML.Result resultObjects = new ML.Result();
           
            ML.Result resultWebApi = new ML.Result();
            resultWebApi.Objects = new List<object>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://www.thecocktaildb.com/api/");

                var responseTask = client.GetAsync("json/v1/1/search.php?f=a" );
                responseTask.Wait();
                
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    //var readTask = result.Content.ReadAsAsync<ML.Coctel>();
                    //var readTask = result.Content.ReadAsStreamAsync();
                    readTask.Wait();
                    
                    foreach ( var resultItem in readTask.Result.ToArray())
                    {
                        ML.Coctel resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Coctel>(resultItem.ToString());
                        resultWebApi.Objects.Add(resultItemList);
                    }
                    ML.Coctel coctel = new ML.Coctel();
                    coctel.cocteles = resultWebApi.Objects;


                    

                    return View(coctel);
                }
            }
            return View(resultWebApi);
        }

        [HttpGet]
        public ActionResult GetBA()
        {
            return View();
        }
    }
}
