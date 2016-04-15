namespace SharedCode.DataManagment
{
    /// <summary>
    /// Interfejs jaki musi implementować obiekt aby mógł być serializowany lub deserializowany za pomocą klasy DataManager.
    /// </summary>
    /// <typeparam name="T">Typ obiektu</typeparam>
    public interface IRestolable<T> where 
        T : new()
    {
        T GetDefaults();
    }
}
