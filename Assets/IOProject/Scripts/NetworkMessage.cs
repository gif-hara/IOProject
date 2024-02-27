namespace IOProject
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NetworkMessage
    {
        public sealed class Helloworld
        {
            public string message;

            override public string ToString()
            {
                return $"message: {message}";
            }
        }
    }
}
