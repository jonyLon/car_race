using System.ComponentModel;
using System.Xml.Linq;

namespace car_race
{
    delegate void Del(Car c);
    abstract class Car
    {
        protected int position = -1;
        public string Name { get; private set; }
        public int MaxSpeed { get; private set; }
        public int CurrentSpeed { get; private set; }
        public int Position { get => position; set {
                if (value >= 100)
                {
                    position = 100;
                    Finish?.Invoke(this);
                } else if (value == 0)
                {
                    position = value;
                    Start?.Invoke(this);
                }
                else
                {
                    position = value;
                }
            } }
        virtual public void Go() {
            Random r = new Random();
            CurrentSpeed = r.Next(MaxSpeed - 50, MaxSpeed);
        }

        public Car(string name, int speed) {
            Name = name;
            MaxSpeed = speed;
            CurrentSpeed = 0;
            
        }
        public event Del? Finish;
        public event Del? Start;

        public void Started()
        {
            Console.WriteLine($"{Name} started race.");
        }
        public void Win()
        {
            Console.WriteLine(Name + " is winner!!");
        }

    }

    class SportCar : Car {
        public SportCar(string name, int speed) : base(name, speed) { }
    }

    class LCar : Car
    {
        public LCar(string name, int speed) : base(name, speed) { }
    }
    class Truck : Car
    {
        public Truck(string name, int speed) : base(name, speed) { }
    }

    class Bus : Car
    {
        public Bus(string name, int speed) : base(name, speed) { }
    }


    class Game
    {

        public List<Car> cars = new List<Car>();
        private bool run = true;
        public Game(params Car[] c) {
            cars.AddRange(c);
        }

        public void Start()
        {
            foreach (var item in cars)
            {
                item.Start += Start;
                item.Finish += Win;
                item.Position = 0;
            }

            while (run)
            {   
                
                foreach (var car in cars)
                {
                    car.Go();
                    car.Position += car.CurrentSpeed / 10;
                    Console.WriteLine($"Car: {car.Name, 20} Position: {car.Position}");
                }
            }
        }
        
        protected void StopRace()
        {
            run = false;
        }

        public void Start(Car c)
        {
            c.Started();
        }
        public void Win(Car c)
        {
            StopRace();
            c.Win();
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Game g = new Game(new SportCar("Lamb", 380), new Truck("Truck", 180), new LCar("JustCar", 220), new Bus("Bus", 120));
            g.Start();

        }
    }
}