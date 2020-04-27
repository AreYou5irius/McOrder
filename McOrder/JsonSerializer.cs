using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;

namespace McOrder
{
    class JsonSerilizer
    {

        /// <summary>
        /// Tager den OC med products og gemmer ned i en fil med et bestemt navn.
        /// </summary>
        /// <param name="products">OC af produkts der skal gemmes</param>
        /// <param name="fileName">Hvilken fil det skal gemmes i, uden endelse</param>
        public static async void SaveProducts(ObservableCollection<Product> products, string fileName)
        {

            try
            {
                //Omskriver OC<product> til en Json string der kan gemmes nede i filen
                string theObjectList = JsonConvert.SerializeObject(products);

                //Definere hvor den skal gemmes, application data.Current.LocalFolder, er den folder systemet har adgang til.
                //Sætter collisionoption så den overskriver ekstisterende hvis der er en af samme navn
                StorageFile localFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName + ".json", CreationCollisionOption.ReplaceExisting);

                //fortæller fil systemet at den skal skrive objectlisten til den fil vi har valgt.
                await FileIO.WriteTextAsync(localFile, theObjectList);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }
        /// <summary>
        /// Giver de objecter fra en fil vi gerne vil have.
        /// </summary>
        /// <param name="fileName">Hvilken fil vi gerne vil have objekterne fra</param>
        /// <returns>En task af typpen OC<products> husk await!!!</returns>
        public static async Task<ObservableCollection<Product>> ReadProducts(string fileName)
        {

            try
            {
                //Vælger filen der skal læses, og tilføjer endelsen
                StorageFile localFile = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName + ".json");

                //fortæller filsystemet at den skal læse texten i documentet, så vi senere kan deserilizere det
                string data = await FileIO.ReadTextAsync(localFile);

                //Deserilizere det, og typecaster det som OC<Product> samtidig med at vi retunere det tilbage fra hvor functionen bliver kaldt.
                return (ObservableCollection<Product>)JsonConvert.DeserializeObject(data, typeof(ObservableCollection<Product>));



            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }
        /// <summary>
        /// Tager alle navnene på filerne
        /// </summary>
        /// <returns>Alle filernesnavne - endelser</returns>
        public static async Task<List<String>> GetButtons()
        {
            //Fortæller hvilken mappe vi skal læse fra, og at vi kun skal have fil navnene
            IReadOnlyList<StorageFile> localFiles = await ApplicationData.Current.LocalFolder.GetFilesAsync();


            if (localFiles.Count == 0)
            {
                NoDataFound();

                localFiles = await ApplicationData.Current.LocalFolder.GetFilesAsync();
            }

            //vil gerne kun retunere en liste af strings så vi ikke skal deformatere det.
            List<String> listOfFiles = new List<String>();

            //fjerner alle fil endelserne og tilføjer dem til den liste der bliver sendt tilbage
            foreach (var file in localFiles)
            {
                listOfFiles.Add(file.Name.Split(".")[0]);
            }

            return listOfFiles;

        }


        public static void NoDataFound()
        {

            ObservableCollection<Product> burgerCollection = new ObservableCollection<Product>(){new Product(30, "Big Mac", @"/Assets/B1.jpg"), new Product(20, "Quater Pounder", @"/Assets/B2.jpg"), new Product(30, "McFeast", @"/Assets/B3.jpg"), new Product(25, "McChicken", @"/Assets/B4.jpg"), new Product(99, "McChicken Suprise", @"/Assets/B5.jpg"), new Product(10, "Cheeseburger", @"/Assets/B6.jpg") };
            ObservableCollection<Product> dessertCollection = new ObservableCollection<Product>() { new Product(10, "Sunday", @"/Assets/Des1.png"), new Product(25, "McFlurry", @"/Assets/Des2.png") };
            ObservableCollection<Product> drinksCollection = new ObservableCollection<Product>(){ new Product(30, "Ice Coffe", @"/Assets/D1.png"), new Product(32, "MilkShake", @"/Assets/D2.png"), new Product(15, "Fanta", @"/Assets/D3.png"), new Product(15, "Cola", @"/Assets/D4.png") };
            ObservableCollection<Product> extrasCollection = new ObservableCollection<Product>() { new Product(12, "Chilli Chesetops", @"/Assets/Ex1.png"), new Product(16, "Chicken nuggets", @"/Assets/Ex2.png"), new Product(8.5, "Pommes", @"/Assets/Ex3.png"), new Product(1200, "Salat", @"/Assets/Ex4.png") };

            SaveProducts(burgerCollection, "Burger");
            SaveProducts(dessertCollection, "Desserter");
            SaveProducts(drinksCollection, "Drikke");
            SaveProducts(extrasCollection, "Diverse");



        }


    }
}