namespace BlazorApp3.Model.Operation
{
    public class Accrual
    {
        private static int _id_counter = 0;
        private readonly int _id;
        private  float _price;
        private string _from;

        private readonly DateTime _datetime;
        public float Price { get { return _price; } }
        public string From { get { return _from; }  set { _from = value; } }
        public string SPrice
        {
            get { return _price.ToString(); }
            set
            {
                _price = float.Parse(value);
            }
        }
        public int Id { get { return _id; } }
        public DateTime DateTime { get { return _datetime; } }
        public Accrual(string sender, float price)
        {
            _id = _id_counter;
            _id_counter++;
            _price = price;
            _from = sender;

            _datetime = DateTime.Now;
        }
        public Accrual()
        {

        }
    }
}
