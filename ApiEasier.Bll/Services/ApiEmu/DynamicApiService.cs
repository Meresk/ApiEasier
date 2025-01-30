﻿using ApiEasier.Bll.Interfaces.ApiConfigure;
using ApiEasier.Bll.Interfaces.ApiEmu;
using ApiEasier.Dal.Helpers;
using ApiEasier.Dal.Interfaces;
using ApiEasier.Dm.Models;

namespace ApiEasier.Bll.Services.ApiEmu
{
    /// <summary>
    /// Сервис для управления динамическими коллекциями в MongoDB.
    /// </summary>
    public class DynamicApiService : IDynamicApiService
    {
        private readonly IDbApiServiceRepository _dbApiServiceRepository;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DynamicApiService"/>.
        /// </summary>
        /// <param name="dbContext">Контекст MongoDB.</param>
        public DynamicApiService(IDbApiServiceRepository dbApiServiceRepository)
        {
            _dbApiServiceRepository = dbApiServiceRepository;
        }

        /// <summary>
        /// Добавляет документ в указанную коллекцию.
        /// </summary>
        /// <param name="apiServiceName">Имя коллекции.</param>
        /// <param name="jsonData">Данные в формате JSON для добавления.</param>
        /// <returns>Словарь, представляющий добавленный документ.</returns>
        /// <exception cref="ArgumentException">Выбрасывается, когда происходит ошибка во время операции.</exception>
        public async Task<DynamicCollectionModel> AddDataAsync(string apiName, string apiEntityName, object jsonData)
        {
            try
            {
                string apiServiceName = apiName.Trim().Replace(" ", "") + "_" + apiEntityName.Trim().Replace(" ", "");

                var result = await _dbApiServiceRepository.CreateAsync(apiServiceName, jsonData);
                return result ?? throw new InvalidOperationException("Не удалось добавить данные");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                throw new ArgumentException("Ошибка при добавлении данных: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Удаляет документ из указанной коллекции по его ID.
        /// </summary>
        /// <param name="collectionName">Имя коллекции.</param>
        /// <param name="id">ID документа для удаления.</param>
        /// <returns>Количество удаленных документов.</returns>
        /// <exception cref="ArgumentException">Выбрасывается, когда происходит ошибка во время операции.</exception>
        //public async Task<long?> DeleteDocFromCollectionAsync(string collectionName, string id)
        //{
        //    try
        //    {
        //        var collections = await _dbContext.GetListCollectionNamesAsync();
        //        if (!collections.Contains(collectionName))
        //            return null;

        //        var collection = _dbContext.GetCollection<BsonDocument>(collectionName);
        //        if (!ObjectId.TryParse(id, out var objectId))
        //            return null;

        //        var result = await collection.DeleteOneAsync(Builders<BsonDocument>.Filter.Eq("_id", objectId));
        //        return result.DeletedCount;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Ошибка: " + ex.Message);
        //        throw new ArgumentException("Ошибка при удалении документа: " + ex.Message, ex);
        //    }
        //}

        /// <summary>
        /// Получает документ из указанной коллекции по его ID.
        /// </summary>
        /// <param name="collectionName">Имя коллекции.</param>
        /// <param name="id">ID документа для получения.</param>
        /// <param name="filters">Дополнительные фильтры для применения.</param>
        /// <returns>Словарь, представляющий документ, или null, если не найден.</returns>
        /// <exception cref="ArgumentException">Выбрасывается, когда происходит ошибка во время операции.</exception>
        public async Task<DynamicCollectionModel> GetDataByIdAsync(string apiName, string apiEntityName, string id, string? filters)
        {
            try
            {
                string apiServiceName = apiName.Trim().Replace(" ", "") + "_" + apiEntityName.Trim().Replace(" ", "");

                var data = await _dbApiServiceRepository.GetDataByIdAsync(apiServiceName, id);

                //var collections = await _dbContext.GetListCollectionNamesAsync();
                //if (!collections.Contains(collectionName))
                //    return null;

                //var collection = _dbContext.GetCollection<BsonDocument>(collectionName);
                //if (!ObjectId.TryParse(id, out var objectId))
                //    return null;

                //FilterDefinition<BsonDocument> documentFilter;
                //try
                //{
                //    documentFilter = BsonSerializer.Deserialize<BsonDocument>(filters);
                //}
                //catch
                //{
                //    documentFilter = Builders<BsonDocument>.Filter.Empty;
                //}

                //var idFilter = Builders<BsonDocument>.Filter.Eq("_id", objectId);
                //var combineFilter = Builders<BsonDocument>.Filter.And(idFilter, documentFilter);
                //var document = await collection.Find(combineFilter).FirstOrDefaultAsync();
                //if (document == null)
                //    return null;

                //var bsonDoc = document.ToDictionary();
                //bsonDoc["_id"] = document["_id"].ToString();


                return bsonDoc;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                throw new ArgumentException("Ошибка при получении документа: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Получает документы из указанной коллекции с применением фильтров.
        /// </summary>
        /// <param name="apiServiceName">Имя коллекции.</param>
        /// <param name="filters">Фильтры для получения документов.</param>
        /// <returns>Список словарей, представляющих документы, или null, если не найдено.</returns>
        /// <exception cref="ArgumentException">Выбрасывается, когда происходит ошибка во время операции.</exception>
        public async Task<List<DynamicCollectionModel>?> GetDataAsync(string apiName, string apiEntityName, string? filters)
        {
            try
            {
                string apiServiceName = apiName.Trim().Replace(" ", "") + "_" + apiEntityName.Trim().Replace(" ", "");

                var data = await _dbApiServiceRepository.GetDataAsync(apiServiceName);

                //var collection = _dbContext.GetCollection<BsonDocument>(collectionName);
                //FilterDefinition<BsonDocument> filterDefinition;

                //try
                //{
                //    filterDefinition = BsonSerializer.Deserialize<BsonDocument>(filters);
                //}
                //catch
                //{
                //    filterDefinition = Builders<BsonDocument>.Filter.Empty;
                //}

                //var documents = await collection.Find(filterDefinition).ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                throw new ArgumentException("Ошибка при получении данных: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Обновляет документ в указанной коллекции по его ID.
        /// </summary>
        /// <param name="collectionName">Имя коллекции.</param>
        /// <param name="id">ID документа для обновления.</param>
        /// <param name="jsonData">Данные в формате JSON для обновления.</param>
        /// <returns>Словарь, представляющий обновленный документ, или null, если документ не найден.</returns>
        /// <exception cref="ArgumentException">Выбрасывается, когда происходит ошибка во время операции.</exception>
        //public async Task<Dictionary<string, object>?> UpdateDocFromCollectionAsync(string collectionName, string id, object jsonData)
        //{
        //    try
        //    {
        //        var collections = await _dbContext.GetListCollectionNamesAsync();
        //        if (!collections.Contains(collectionName))
        //            return null;

        //        var collection = _dbContext.GetCollection<BsonDocument>(collectionName);
        //        if (!ObjectId.TryParse(id, out var objectId))
        //            return null;

        //        var document = await collection.Find(Builders<BsonDocument>.Filter.Eq("_id", objectId)).FirstOrDefaultAsync();
        //        if (document == null)
        //            return null;

        //        document.Remove("_id"); // Удаляем _id из данных, чтобы не перезаписать его

        //        // Используем ReplaceOne для замены всего документа
        //        var replacementResult = await collection.ReplaceOneAsync(
        //            Builders<BsonDocument>.Filter.Eq("_id", objectId),
        //            BsonDocument.Parse(jsonData.ToString())
        //        );

        //        if (replacementResult.ModifiedCount > 0)
        //        {
        //            // Если документ заменен, возвращаем обновленный документ
        //            var updatedDocument = await collection.Find(Builders<BsonDocument>.Filter.Eq("_id", objectId)).FirstOrDefaultAsync();
        //            if (updatedDocument != null)
        //            {
        //                var bsonDoc = updatedDocument.ToDictionary();
        //                bsonDoc["_id"] = updatedDocument["_id"].ToString();
        //                return bsonDoc;
        //            }
        //        }

        //        return null; // Если документ не был заменен
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Ошибка: " + ex.Message);
        //        throw new ArgumentException("Ошибка при обновлении документа: " + ex.Message, ex);
        //    }
        //}

        /// <summary>
        /// Удаляет коллекции по удалении конфигурации API-сервиса.
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса</param>
        //public async Task DeleteCollectionsByDeletedApiServiceAsync(string apiServiceName)
        //{
        //    foreach (var collection in (await _dbContext.GetListCollectionNamesAsync()).Where(c => c.Split("_")[0] == apiServiceName))
        //    {
        //        await _dbContext.DropCollectionAsync(collection);
        //    }
        //}

        /// <summary>
        /// Удаляет коллекцию по изменении конфигурации API-сервиса.
        /// </summary>
        /// <param name="apiServiceName">Имя API-сервиса</param>
        //public async Task DeleteCollectionsByChangedApiServiceAsync(string apiServiceName)
        //{
        //    var apiService = await _configFileApiService.DeserializeApiServiceAsync(apiServiceName);
        //    foreach (var collection in (await _dbContext.GetListCollectionNamesAsync()).Where(c => c.Split("_")[0] == apiServiceName && !apiService.Entities.Any(e => e.Name == c.Split("_")[1])))
        //    {
        //        await _dbContext.DropCollectionAsync(collection);
        //    }
        //}

        /// <summary>
        /// Переименовывает коллекцию по новому имени API-сервиса.
        /// </summary>
        /// <param name="oldApiServiceName">Старое имя API-сервиса</param>
        /// <param name="apiServiceName">Новое имя API-сервиса</param>
        //public async Task RenameCollectionsByApiServiceAsync(string oldApiServiceName, string apiServiceName)
        //{
        //    var list = await _dbContext.GetListCollectionNamesAsync();
        //    var temp = list.FirstOrDefault().Split("_")[0];
        //    foreach (var collection in (await _dbContext.GetListCollectionNamesAsync()).Where(c => c.Split("_")[0] == oldApiServiceName))
        //    {
        //        await _dbContext.RenameCollectionAsync(collection, $"{apiServiceName}_{collection.Split("_")[1]}");
        //    }
        //}

        /// <summary>
        /// Находит неиспользуемые коллекции по всем файлам конфигурации и удаляет их
        /// </summary>
        //public async Task DeleteTrashCollectionAsync()
        //{
        //    var apiServices = await _configFileApiService.GetApiServicesAsync();
        //    foreach (var collection in (await _dbContext.GetListCollectionNamesAsync())
        //        .Where(c => !apiServices.Any(asn => c.Split("_")[0] == asn.Name) || !apiServices.Any(ase => ase.Entities.Any(e => e.Name == c.Split("_")[1]))))
        //        await _dbContext.DropCollectionAsync(collection);
        //}
    }
}

