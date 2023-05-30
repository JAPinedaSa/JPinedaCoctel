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

                var responseTask = client.GetAsync("json/v1/1/search.php?f=a");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    //var readTask = result.Content.ReadAsStringAsync();
                    var readTask = result.Content.ReadAsAsync<ML.Coctel>();
                    //var readTask = result.Content.ReadAsStreamAsync();
                    readTask.Wait();

                    foreach (var resultItem in readTask.Result.drinks)
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

        [HttpGet]
        public ActionResult GetAllDynamic()
        {
            //Letra = "a";
            ML.Result resultObjects = new ML.Result();

            ML.Result resultWebApi = new ML.Result();
            resultWebApi.Objects = new List<object>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://www.thecocktaildb.com/api/");

                var responseTask = client.GetAsync("json/v1/1/search.php?f=a");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    //var readTask = result.Content.ReadAsStringAsync();
                    var readTask = result.Content.ReadAsAsync<dynamic>();
                    //var readTask = result.Content.ReadAsStreamAsync();
                    readTask.Wait();

                    foreach (dynamic resultItem in readTask.Result.drinks)
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
        public ActionResult IngredientesGetByCoctel(int idCoctel)
        {
            
            ML.Coctel ingredientes = new ML.Coctel();
            ML.Result resultIngredientes = new ML.Result();
            ML.Result resultWebApi = new ML.Result();
           
            resultWebApi.Objects = new List<object>();
            resultIngredientes.Objects = new List<object>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://www.thecocktaildb.com/api/");

                var responseTask = client.GetAsync("json/v1/1/lookup.php?i=" + idCoctel);
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    //var readTask = result.Content.ReadAsStringAsync();
                    var readTask = result.Content.ReadAsAsync<dynamic>();
                    //var readTask = result.Content.ReadAsStreamAsync();
                    readTask.Wait();

                    foreach (dynamic resultItem in readTask.Result.drinks)
                    {
                        ML.Coctel resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Coctel>(resultItem.ToString());
                        resultWebApi.Objects.Add(resultItemList);
                        ingredientes.strIngredient1 = resultItem.strIngredient1;
                        ingredientes.strIngredient2 = resultItem.strIngredient2;
                        ingredientes.strIngredient3 = resultItem.strIngredient3;
                        ingredientes.strIngredient4 = resultItem.strIngredient4;
                        ingredientes.strIngredient5 = resultItem.strIngredient5;
                        ingredientes.strIngredient6 = resultItem.strIngredient6;
                        ingredientes.strIngredient7 = resultItem.strIngredient7;
                        ingredientes.strIngredient8 = resultItem.strIngredient8;
                        ingredientes.strIngredient9 = resultItem.strIngredient9;
                        ingredientes.strIngredient10 = resultItem.strIngredient10;
                        ingredientes.strIngredient11 = resultItem.strIngredient11;
                        ingredientes.strIngredient12 = resultItem.strIngredient12;
                        ingredientes.strIngredient13 = resultItem.strIngredient13;
                        ingredientes.strIngredient14 = resultItem.strIngredient14;
                        ingredientes.strIngredient15 = resultItem.strIngredient15;
                        resultIngredientes.Objects.Add(ingredientes);
                    }
                    ML.Coctel coctel = new ML.Coctel();
                    coctel.Ingredientess = new ML.Ingredientes();
                    
                    coctel.Ingredientess.Ingrediente = resultIngredientes.Objects;
                    coctel.Ingredientess.Imagen = "https://www.thecocktaildb.com/images/ingredients/";




                    return View(coctel);
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult GetAll(ML.Coctel cocktail)
        {
            if (cocktail.strDrink == null && cocktail.strIngredient == null)
            {
                //Letra = "a";
                ML.Result resultWebApi = new ML.Result();
                resultWebApi.Objects = new List<object>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://www.thecocktaildb.com/api/");

                    var responseTask = client.GetAsync("json/v1/1/search.php?f=a");
                    responseTask.Wait();

                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        //var readTask = result.Content.ReadAsStringAsync();
                        var readTask = result.Content.ReadAsAsync<ML.Coctel>();
                        //var readTask = result.Content.ReadAsStreamAsync();
                        readTask.Wait();

                        foreach (var resultItem in readTask.Result.drinks)
                        {
                            ML.Coctel resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Coctel>(resultItem.ToString());
                            resultWebApi.Objects.Add(resultItemList);
                        }
                        ML.Coctel coctel = new ML.Coctel();
                        coctel.cocteles = resultWebApi.Objects;




                        return View(coctel);
                    }
                }
            }
            else
            {
                if (cocktail.strIngredient == null)
                {
                    ML.Result resultWebApi = new ML.Result();
                    resultWebApi.Objects = new List<object>();
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://www.thecocktaildb.com/api/");

                        var responseTask = client.GetAsync("json/v1/1/search.php?s="+ cocktail.strDrink);
                        responseTask.Wait();

                        var result = responseTask.Result;

                        if (result.IsSuccessStatusCode)
                        {
                            //var readTask = result.Content.ReadAsStringAsync();
                            var readTask = result.Content.ReadAsAsync<ML.Coctel>();
                            //var readTask = result.Content.ReadAsStreamAsync();
                            readTask.Wait();

                            foreach (var resultItem in readTask.Result.drinks)
                            {
                                ML.Coctel resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Coctel>(resultItem.ToString());
                                resultWebApi.Objects.Add(resultItemList);
                            }
                            ML.Coctel coctel = new ML.Coctel();
                            coctel.cocteles = resultWebApi.Objects;




                            return View(coctel);
                        }
                    }
                }
                else
                {
                    
                   

                    ML.Result resultWebApi = new ML.Result();
                    resultWebApi.Objects = new List<object>();
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://www.thecocktaildb.com/api/");

                        var responseTask = client.GetAsync("json/v1/1/filter.php?i=" + cocktail.strIngredient);
                        responseTask.Wait();

                        var result = responseTask.Result;

                        if (result.IsSuccessStatusCode)
                        {
                            //var readTask = result.Content.ReadAsStringAsync();
                            var readTask = result.Content.ReadAsAsync<ML.Coctel>();
                            //var readTask = result.Content.ReadAsStreamAsync();
                            readTask.Wait();

                            foreach (var resultItem in readTask.Result.drinks)
                            {
                                ML.Coctel resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Coctel>(resultItem.ToString());
                                resultWebApi.Objects.Add(resultItemList);
                            }
                            ML.Coctel coctel = new ML.Coctel();
                            coctel.cocteles = resultWebApi.Objects;




                            return View(coctel);
                        }
                    }
                }
            }
           
            return View();
        }

    }
}
