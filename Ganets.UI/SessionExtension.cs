using System.Text.Json;

namespace Ganets.UI
{
    public static class SessionExtension
    {
        // Метод для сохранения объектов в сессии
        public static void Set<T>(this ISession session, string key, T item)
        {
            var serializedItem = JsonSerializer.Serialize(item); // Преобразуем объект в JSON
            session.SetString(key, serializedItem); // Сохраняем в сессии
        }

        // Метод для получения объектов из сессии
        public static T Get<T>(this ISession session, string key)
        {
            var item = session.GetString(key); // Получаем данные по ключу
            return item == null ? Activator.CreateInstance<T>() : JsonSerializer.Deserialize<T>(item);
        }
    }
}
