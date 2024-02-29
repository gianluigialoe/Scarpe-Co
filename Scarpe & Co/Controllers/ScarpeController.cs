using Microsoft.AspNetCore.Mvc;
using Scarpe___Co.Models;
using Microsoft.Data.SqlClient;
using static System.Formats.Asn1.AsnWriter;
namespace Scarpe___Co.Controllers
{
    public class ScarpeController : Controller
    {

        // metodo per connettere al database
        private string connString = "Server=PCGIANLUIGI\\SQLEXPRESS; Initial Catalog=PROVADTI N MVC; Integrated Security=true; TrustServerCertificate=True";

        [HttpGet]
        public IActionResult Index()
        {
            var conn = new SqlConnection(connString);
            List<Scarpe> scarpeList = new List<Scarpe>(); // Correggo il nome della lista

            try
            {
                conn.Open();

                var command = new SqlCommand("SELECT * FROM Table_1", conn);

                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var scarpa = new Scarpe() // Correggo il nome dell'oggetto
                        {
                            ScarpeId = (int)reader["ScarpeId"], // Correggo il nome del campo
                            Name = reader["Name"].ToString(),
                            Price = (int)reader["Price"], // Considerando il prezzo come decimal
                            Descrizione = reader["Descrizione"].ToString(),
                            ImmagineDiCopertina = reader["ImmagineDiCopertina"].ToString(),
                            Immagine1 = reader["Immagine1"].ToString(),
                            Immagine2 = reader["Immagine2"].ToString()
                        };
                        scarpeList.Add(scarpa); // Correggo il nome della lista
                    }
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
            finally
            {
                conn.Close(); // Assicurati di chiudere la connessione
            }

            return View(scarpeList); // Correggo il nome della lista
        }


        // pagina con un form per l'aggiunta di un nuovo pesce
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // indirizzo per gestire il submit del form della pagina Add
        [HttpPost]
        [HttpPost]
        public IActionResult Add(Scarpe scarpe, IFormFile immagineDiCopertina, IFormFile immagine1, IFormFile immagine2)
        {
            var error = true;
            var conn = new SqlConnection(connString);

            try
            {
                // Validare i dati

                conn.Open();

                // Salvare l'immagine di copertina
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                string fileNameDiCopertina = Path.GetFileName(immagineDiCopertina.FileName);
                string fullFilePathDiCopertina = Path.Combine(path, fileNameDiCopertina);

                using (FileStream stream = new FileStream(fullFilePathDiCopertina, FileMode.Create))
                {
                    immagineDiCopertina.CopyTo(stream);
                }

                // Salvare l'immagine 1
                string fileName1 = Path.GetFileName(immagine1.FileName);
                string fullFilePath1 = Path.Combine(path, fileName1);

                using (FileStream stream = new FileStream(fullFilePath1, FileMode.Create))
                {
                    immagine1.CopyTo(stream);
                }

                // Salvare l'immagine 2
                string fileName2 = Path.GetFileName(immagine2.FileName);
                string fullFilePath2 = Path.Combine(path, fileName2);

                using (FileStream stream = new FileStream(fullFilePath2, FileMode.Create))
                {
                    immagine2.CopyTo(stream);
                }

                // Creare il comando
                var command = new SqlCommand(@"
            INSERT INTO Table_1
            (Name, Price, Descrizione, ImmagineDiCopertina, Immagine1, Immagine2) VALUES
            (@name, @price, @descrizione, @immagineDiCopertina, @immagine1, @immagine2)", conn);

                command.Parameters.AddWithValue("@name", scarpe.Name);
                command.Parameters.AddWithValue("@price", scarpe.Price);
                command.Parameters.AddWithValue("@descrizione", scarpe.Descrizione);
                command.Parameters.AddWithValue("@immagineDiCopertina", fileNameDiCopertina);
                command.Parameters.AddWithValue("@immagine1", fileName1);
                command.Parameters.AddWithValue("@immagine2", fileName2);

                // Eseguire il comando
                var nRows = command.ExecuteNonQuery();
                error = false;
            }
            catch (Exception ex)
            {
                // Gestire eccezioni, log o altro se necessario
            }
            finally
            {
                conn.Close();
            }

            if (error)
            {
                return View("Error");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Delete(int scarpeId)
        {
            var error = true;
            var deletedScarpe = new Scarpe();
            var conn = new SqlConnection(connString);

            try
            {
                conn.Open();

                // Leggere i dati della scarpe da eliminare
                var commandRead = new SqlCommand("SELECT * FROM Table_1 WHERE ScarpeId=@scarpeId", conn);
                commandRead.Parameters.AddWithValue("@scarpeId", scarpeId);

                var reader = commandRead.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    deletedScarpe.ScarpeId = (int)reader["ScarpeId"];
                    deletedScarpe.Name = reader["Name"].ToString();
                    deletedScarpe.Price = (int)reader["Price"];
                    deletedScarpe.Descrizione = reader["Descrizione"].ToString();
                    deletedScarpe.ImmagineDiCopertina = reader["ImmagineDiCopertina"].ToString();
                    deletedScarpe.Immagine1 = reader["Immagine1"].ToString();
                    deletedScarpe.Immagine2 = reader["Immagine2"].ToString();
                }

                reader.Close();

                // Eliminare la scarpe
                var commandDelete = new SqlCommand("DELETE FROM Table_1 WHERE ScarpeId=@scarpeId", conn);
                commandDelete.Parameters.AddWithValue("@scarpeId", scarpeId);

                var nRows = commandDelete.ExecuteNonQuery();

                if (nRows > 0)
                {
                    error = false;

                    // Eliminare anche le immagini
                    if (!string.IsNullOrEmpty(deletedScarpe.ImmagineDiCopertina))
                    {
                        string fullPathDiCopertina = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", deletedScarpe.ImmagineDiCopertina);
                        if (System.IO.File.Exists(fullPathDiCopertina))
                        {
                            System.IO.File.Delete(fullPathDiCopertina);
                        }
                    }

                    // Ripetere lo stesso processo per Immagine1
                    if (!string.IsNullOrEmpty(deletedScarpe.Immagine1))
                    {
                        string fullPathImmagine1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", deletedScarpe.Immagine1);
                        if (System.IO.File.Exists(fullPathImmagine1))
                        {
                            System.IO.File.Delete(fullPathImmagine1);
                        }
                    }

                    // Ripetere lo stesso processo per Immagine2
                    if (!string.IsNullOrEmpty(deletedScarpe.Immagine2))
                    {
                        string fullPathImmagine2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", deletedScarpe.Immagine2);
                        if (System.IO.File.Exists(fullPathImmagine2))
                        {
                            System.IO.File.Delete(fullPathImmagine2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Gestire eccezioni, log o altro se necessario
            }
            finally
            {
                conn.Close();
            }

            if (!error)
            {
                TempData["MessageSuccess"] = $"Le scarpe {deletedScarpe.Name} sono state eliminate";
                return RedirectToAction("Index", "Scarpe");  // Reindirizza alla pagina Index del controller Scarpe
            }

            TempData["MessageError"] = "C'è stato un problema durante l'eliminazione";
            return RedirectToAction("Index", "Scarpe");  // Reindirizza alla pagina Index del controller Scarpe
        }
        public IActionResult Delete(int? id)
        {
            if (id is not null)
            {
                var scarpe = GetById((int)id);  // Assumendo che tu abbia un metodo GetById per ottenere un'entità Scarpe dal tuo database
                return View(scarpe);
            }
            return RedirectToAction("Index");
        }

    }
}



        //METODO DATABASESTATICO
        //    public IActionResult Add()
        //    {
        //        return View();
        //    }
        //    // indirizzo per gestire il submit del form della pagina Add

//    [HttpPost]
//    public IActionResult Add(string name, int price, string descrizione, string immagineDiCopertina, string immagine1, string immagine2)
//    {
//        // Validare i dati
//        // ... (inserire qui la logica di validazione)

//        // Aggiungere le scarpe
//        var scarpe = StaticDb.Add(name, price, descrizione, immagineDiCopertina, immagine1, immagine2);

//        // Redirect all'azione "Details" di "Scarpe" con l'ID delle scarpe appena aggiunte
//        return RedirectToAction("Index", "Scarpe");
//    }
//    [HttpGet]
//    // Gestisce una richiesta HTTP di tipo GET per l'azione "Edit"
//    // Accetta un parametro "id" dalla route che rappresenta l'ID dell'oggetto Fish da modificare
//    public IActionResult Edit([FromRoute] int? id)
//    {
//        // Se l'ID è nullo, reindirizza l'utente all'azione "Index" del controller "Fish"
//        if (id is null)
//            return RedirectToAction("Edit", "Scarpe");

//        // Ottiene l'oggetto Fish corrispondente all'ID dalla lista tramite il metodo GetById
//        var Scarpe = StaticDb.GetById(id);

//        // Se l'oggetto Fish non è stato trovato, reindirizza l'utente alla vista "Error"
//        if (Scarpe is null)
//            return View("Error");

//        // Se l'oggetto Fish è stato trovato, restituisce la vista "Edit" con l'oggetto Fish come modello
//        return View(Scarpe);
//    }
//}


//}