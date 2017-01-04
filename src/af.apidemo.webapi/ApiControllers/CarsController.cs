using af.apidemo.webapi.Models;
using JSend.WebApi;
using JSend.WebApi.Results;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace af.apidemo.webapi.ApiControllers
{
    public class CarsController : JSendApiController
    {
        CloudStorageAccount account = new CloudStorageAccount(
            new StorageCredentials("afdemo4062", "RC0CKH8jxas8de8/XI3w+WhBMWSph81umN8Q5ayPD0v9g52pT6Xp7gfRiHCZwteGjvLLMLLNlzqH4KoFdnCHgw=="), true);

        [HttpGet]
        [ResponseType(typeof(JSendOkResult<List<Car>>))]
        public IHttpActionResult Get()
        {
            CloudTableClient tableClient = account.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Cars");

            List<CarEntity> cars = new List<CarEntity>();

            TableContinuationToken token = null;
            var entities = new List<CarEntity>();
            do
            {
                var queryResult = table.ExecuteQuerySegmented(new TableQuery<CarEntity>(), token);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);

            if (entities.Count == 0)
                return JSendOk<List<Car>>(null);

            return JSendOk<List<Car>>(entities.Select(c => new Car(c)).ToList());
        }

        [HttpGet]
        [ResponseType(typeof(JSendOkResult<Car>))]
        public IHttpActionResult GetById(int id)
        {
            CloudTableClient tableClient = account.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Cars");

            var query = new TableQuery<CarEntity>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, id.ToString()));

            var entity = table.ExecuteQuery(query).FirstOrDefault();
            if (entity == null)
                return JSendNotFound();
            
            return JSendOk<Car>(new Car(entity));
        }

        [HttpPut]
        [ResponseType(typeof(JSendOkResult<Car>))]
        public IHttpActionResult Put(int id, [FromBody]Car car)
        {
            CloudTableClient tableClient = account.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Cars");

            var c = new CarEntity(car);

            TableOperation retrieveOperation = TableOperation.Retrieve<CarEntity>(c.PartitionKey, c.RowKey);
            
            TableResult retrievedResult = table.Execute(retrieveOperation);
            
            CarEntity updateEntity = (CarEntity)retrievedResult.Result;

            updateEntity.Year = c.Year;
            updateEntity.Color = c.Color;
            
            TableOperation updateOperation = TableOperation.Replace(updateEntity);
            
            table.Execute(updateOperation);


            return JSendOk<Car>(new Car(updateEntity));
        }

        [HttpPost]
        [ResponseType(typeof(JSendOkResult<Car>))]
        public IHttpActionResult Post(Car car)
        {
            CloudTableClient tableClient = account.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Cars");
            
            TableContinuationToken token = null;
            var entities = new List<CarEntity>();
            do
            {
                var queryResult = table.ExecuteQuerySegmented(new TableQuery<CarEntity>(), token);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);

            var highestCarId = entities.Select(e => new Car(e)).OrderByDescending(c => c.CarId);
            car.CarId = highestCarId.First().CarId + 1;
            var entity = new CarEntity(car);
            TableOperation insertOperation = TableOperation.Insert(entity);
            
            table.Execute(insertOperation);

            return JSendOk<Car>(car);
        }

        [HttpDelete]
        [ResponseType(typeof(JSendOkResult<bool>))]
        public IHttpActionResult Delete(int id)
        {
            CloudTableClient tableClient = account.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Cars");
            
            try
            {
                var query = new TableQuery<CarEntity>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, id.ToString()));

                var entity = table.ExecuteQuery(query).FirstOrDefault();

                TableOperation deleteOperation = TableOperation.Delete(entity);

                table.Execute(deleteOperation);
                
                return JSendOk(true);
            }
            catch (Exception ex)
            {
                return JSendOk(false);
            }
        }
    }
}