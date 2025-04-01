namespace BIS.Data
{
    public struct ValueData<T>
    {
        public T value;

        public ValueData(T value)
        {
            this.value = value;
        }
    }
}
