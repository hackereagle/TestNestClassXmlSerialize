using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Diagnostics;

public class Program
{
    public enum AlgParameterType
    { 
        Int,
        Double
    }

    [XmlRoot]
#pragma warning disable CS0659, CS0661 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Parameter
#pragma warning restore CS0659, CS0661 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public Parameter() 
        {
            Name = string.Empty;
            Value = 0;
            Description = string.Empty;
            ParameterType = AlgParameterType.Int;
        }
        public Parameter (string name, AlgParameterType type, object value, string des) 
        {
            ParameterType = type;
            Value = value;
            Description = des;
            Name = name;
        }

        [XmlAttribute("ParameterName")]
        public string Name { get; set; }

        [XmlElement]
        public AlgParameterType ParameterType { get; set; }

        [XmlElement]
        public object Value { get; set; }

        [XmlElement]
        public string Description { get; set; }

        public override bool Equals(Object? obj)
        {
            bool isSameType = obj is Parameter;
            bool isEqual = false;
            if (isSameType)
            {
                isEqual = this.ParameterType == ((Parameter)obj!).ParameterType &&
                          this.Name == ((Parameter)obj!).Name;

                if (isEqual && this.ParameterType == AlgParameterType.Int)
                    isEqual = isEqual && (int)this.ParameterType == (int)((Parameter)obj!).ParameterType;
                else if (isEqual && this.ParameterType == AlgParameterType.Double)
                    isEqual = isEqual && (double)this.ParameterType == (double)((Parameter)obj!).ParameterType;
            }

            return isSameType && isEqual;
        }

        public static bool operator ==(Parameter left, Parameter right)
        { 
            return left.Equals(right);
        }
        public static bool operator !=(Parameter left, Parameter right)
        { 
            return !left.Equals(right);
        }
    }

    [XmlInclude(typeof(Parameter))]
    [XmlRoot]
    public class Algorithm
    {
        public Algorithm()
        {
            Name = string.Empty;
            _parameters = new ObservableCollection<Parameter>();
            LightIntensity = -1;
        }

        public Algorithm(string name, List<Parameter> param, int intensity)
        {
            Name = name;
            _parameters = new ObservableCollection<Parameter>(param);
            LightIntensity = intensity;
        }

        [XmlAttribute("AlgorithmName")]
        public string Name { get; set; }

        ObservableCollection<Parameter> _parameters;
        [XmlArray]
        public ObservableCollection<Parameter> AlgorithmParameters => _parameters;

        [XmlElement]
        public int LightIntensity { get; set; }

    }

    private static void Main(string[] args)
    {
        // Initialize algorithm
        Algorithm alg = new Algorithm(
            "Algorithm1",
            new List<Parameter>()
            {
                new Parameter("Param1", AlgParameterType.Int, 100, "This is Param1"),
                new Parameter("Param2", AlgParameterType.Double, 200.0, "This is Param2")
            }, 
            100);

        // Testing xml serialize/deserialize
        Console.WriteLine("===== Test serialization! =====");
        string path = System.AppDomain.CurrentDomain.BaseDirectory;
        string xmlFile = path + "\\Test.xml";
        XmlSerializer xml = new XmlSerializer(typeof(Algorithm));
        using (StreamWriter writer = new StreamWriter(xmlFile))
        {
            xml.Serialize(writer, alg);
            writer.Close();
        }

        Console.WriteLine("Finished serialization!");

        Console.WriteLine("\n===== Test deserialization! =====");
        Algorithm algRead;
        XmlSerializer xmlRead = new XmlSerializer(typeof(Algorithm));
        using (StreamReader reader = new StreamReader(xmlFile))
        { 
            algRead = (Algorithm)xmlRead.Deserialize(reader)!;
        }
        //algRead.AlgorithmParameters[0].Value = 1000;
        Console.WriteLine("Finished deserialization!");
        Console.WriteLine("Check read Algorithm with original object");
        Debug.Assert(algRead.Name == alg.Name);
        Debug.Assert(algRead.AlgorithmParameters[0] == alg.AlgorithmParameters[0]);
        Debug.Assert(algRead.AlgorithmParameters[1] == alg.AlgorithmParameters[1]);
        Debug.Assert(algRead.LightIntensity == alg.LightIntensity);
        Console.WriteLine("All value is same!");

        Console.ReadLine();
    }
}