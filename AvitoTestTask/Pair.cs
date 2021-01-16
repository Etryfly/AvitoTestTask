namespace AvitoTestTask
{
    public class Pair
    {
        public string key { get; set; }
        public string value { get; set; }
        public int ttl { get; set; }

        /*   public Pair(string key, string value, int ttl)
           {
               this.key = key;
               this.value = value;
               this.ttl = ttl;

           }*/

        public override bool Equals(object obj)
        {
            return obj is Pair pair &&
                   key == pair.key &&
                   value == pair.value &&
                   ttl == pair.ttl;
        }
    }
}
