namespace QAToolKit.Swagger.AspNet.Demo.Models
{
    public class BicycleBrochure
    {
        public Metadata Metadata { get; set; }
        public BicycleImage Image { get; set; }
    }

    public class Metadata
    {
        public string Name { get; set; }
        public int Year { get; set; }
    }
}
