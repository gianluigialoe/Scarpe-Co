using static System.Formats.Asn1.AsnWriter;

namespace Scarpe___Co.Models
{
    public static class StaticDb
    {
        private static int _maxId = 3;
        private static List<Scarpe> _Scarpes = new List<Scarpe>
    {
        new Scarpe() { ScarpeId = 1, Name = "ADIDAS 00S", Price = 1500, Descrizione = "CAMPUS 00S BELLISSIME,COMODISSIME,SEMPLICISSIME ,BELLISSIME", ImmagineDiCopertina= "//hhttp immagine di copertina", Immagine1="//HHTTP IMMAGINE1", Immagine2 = "//HHTTP IMMAGINE2", },
        new Scarpe() { ScarpeId = 2, Name = "ADIDAS 00S", Price = 1500, Descrizione = "CAMPUS 00S BELLISSIME,COMODISSIME,SEMPLICISSIME ,BELLISSIME", ImmagineDiCopertina = "//hhttp immagine di copertina",Immagine1 = "//HHTTP IMMAGINE1",Immagine2 = "//HHTTP IMMAGINE2", },
        new Scarpe() { ScarpeId = 3, Name = "ADIDAS 00S", Price = 1500, Descrizione = "CAMPUS 00S BELLISSIME,COMODISSIME,SEMPLICISSIME ,BELLISSIME", ImmagineDiCopertina = "//hhttp immagine di copertina",Immagine1 = "//HHTTP IMMAGINE1", Immagine2 = "//HHTTP IMMAGINE2", },
    };

        public static List<Scarpe> GetAll()
        {
            foreach (var scarpe in _Scarpes)
            {
                // Fai qualcosa con l'oggetto scarpe, ad esempio aggiungilo a un'altra lista o esegui altre operazioni
            }

            return _Scarpes;
        }

        public static Scarpe Add(string name, int price, string descrizione, string immagineDiCopertina, string immagine1, string immagine2)
        {
            _maxId++;
            var scarpe = new Scarpe()
            {
                ScarpeId = _maxId,
                Name = name,
                Price = price,
                Descrizione = descrizione,
                ImmagineDiCopertina = immagineDiCopertina,
                Immagine1 = immagine1,
                Immagine2 = immagine2,
            };

            _Scarpes.Add(scarpe);
            return scarpe;
        }

        // Ottiene un oggetto Scarpa dalla lista basato sull'ID fornito
        // Restituisce l'oggetto Scarpa corrispondente all'ID o null se l'ID è nullo o non è stato trovato
        public static Scarpe? GetById(int? id)
        {
            // Se l'ID è nullo, restituisci immediatamente null
            if (id is null)
                return null;

            // Itera attraverso gli elementi nella lista _fishes utilizzando un ciclo for
            for (int i = 0; i < _Scarpes.Count; i++)
            {
                // Ottieni l'elemento corrente dalla lista
                Scarpe Scarpe = _Scarpes[i];

                // Verifica se l'ID dell'elemento corrente corrisponde all'ID fornito
                if (Scarpe.ScarpeId == id)
                {
                    // Se c'è una corrispondenza, restituisci l'oggetto Fish
                    return Scarpe;
                }
            }

            // Se l'ID non è stato trovato nella lista, restituisci null
            return null;
        }







        // Modifica un oggetto Fish esistente nella lista
        // Restituisce l'oggetto Fish modificato o null se non viene trovato
        public static Scarpe? Modify(Scarpe Scarpe)
        {
            // Itera attraverso ogni oggetto Fish nella lista _fishes
            foreach (var ScarpeInList in _Scarpes)
            {
                // Verifica se l'identificativo dell'oggetto corrente coincide con l'identificativo dell'oggetto passato come parametro
                if (ScarpeInList.ScarpeId == Scarpe.ScarpeId)
                {
                    // Se c'è una corrispondenza, aggiorna le proprietà dell'oggetto corrente con quelle dell'oggetto passato
                    ScarpeInList.Name = Scarpe.Name;
                    ScarpeInList.Price = Scarpe.Price;
                    ScarpeInList.Descrizione = Scarpe.Descrizione;
                    ScarpeInList.ImmagineDiCopertina = Scarpe.ImmagineDiCopertina;
                    ScarpeInList.Immagine1= Scarpe.Immagine1;
                    ScarpeInList.Immagine2= Scarpe.Immagine2;

                    // Restituisce l'oggetto Fish modificato
                    return ScarpeInList;
                }
            }

            // Se non viene trovato un oggetto con l'identificativo corrispondente, restituisce null
            return null;
        }

    }
}