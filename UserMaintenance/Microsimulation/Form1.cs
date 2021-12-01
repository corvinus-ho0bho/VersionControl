using Microsimulation.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Microsimulation
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();
        Random rng = new Random(1234);
        List<int> males = new List<int>();
        List<int> females = new List<int>();
        public Form1()
        {
            InitializeComponent();

            Population = GetPopulation(@"C:\Temp\nép-teszt.csv");
            BirthProbabilities = GetBirthProbabilities(@"C:\Temp\születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\Temp\halál.csv");

            // dataGridView1.DataSource = Population;

        }
        public void Simulation()
        {
            richTextBox1.Clear();
            males.Clear();
            females.Clear();

            var lastyear = numericUpDown1.Value;
            for (int year = 2005; year <= lastyear; year++)
            {
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(year, Population[i]);
                }

                int NumberOfMales = (from x in Population
                                     where x.Gender == Gender.Male && x.IsAlive
                                     select x).Count();
                int NumberOfFemales = (from x in Population
                                       where x.Gender == Gender.Female && x.IsAlive
                                       select x).Count();
                males.Add(NumberOfMales);
                females.Add(NumberOfFemales);
            }
        }
        private void SimStep(int year, Person person)
        {
            
            if (!person.IsAlive) return;

            
            byte age = (byte)(year - person.BirthYear);

            
            double pDeath = (from x in DeathProbabilities
                             where x.Gender == person.Gender && x.Age == age
                             select x.DProbability).FirstOrDefault();
           
            if (rng.NextDouble() <= pDeath)
                person.IsAlive = false;

            
            if (person.IsAlive && person.Gender == Gender.Female)
            {
               
                double pBirth = (from x in BirthProbabilities
                                 where x.Age == age
                                 select x.BProbability).FirstOrDefault();
                
                if (rng.NextDouble() <= pBirth)
                {
                    Person újszülött = new Person();
                    újszülött.BirthYear = year;
                    újszülött.NbrOfChildren = 0;
                    újszülött.Gender = (Gender)(rng.Next(1, 3));
                    Population.Add(újszülött);
                }
            }
        }
        public List<Person> GetPopulation(string csvpath)
        {
            List<Person> population = new List<Person>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new Person()
                    {
                        BirthYear = int.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        NbrOfChildren = int.Parse(line[2])
                    });
                }
            }

            return population;
        }
        public List<BirthProbability> GetBirthProbabilities(string csvpath)
        {
            List<BirthProbability> birthProbabilities = new List<BirthProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    birthProbabilities.Add(new BirthProbability()
                    {
                        Age = int.Parse(line[0]),
                        NbrOfChildren = int.Parse(line[1]),
                        BProbability = double.Parse(line[2])
                    });
                }
            }

            return birthProbabilities;
        }

        public List<DeathProbability> GetDeathProbabilities(string csvpath)
        {
            List<DeathProbability> deathProbabilities = new List<DeathProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    deathProbabilities.Add(new DeathProbability()
                    {
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[0]),
                        Age = int.Parse(line[1]),
                        DProbability = double.Parse(line[2])
                    });
                }
            }

            return deathProbabilities;
        }
        public void DisplayResults()
        {
            for (int i = 2005; i <= numericUpDown1.Value; i++)
            {
                richTextBox1.Text += "Szimulacios ev: " + i.ToString() + "\n" +
                                    "\t" + "Ferfiak szama: " + males[i - 2005] + "\n" +
                                    "\t" + "Nok szama: " + females[i - 2005] + "\n";
            }
        }
        private void startButton_Click(object sender, EventArgs e)
        {
            Simulation();
            DisplayResults();
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
                Population = GetPopulation(textBox1.Text);
            }
        }
    }
}
