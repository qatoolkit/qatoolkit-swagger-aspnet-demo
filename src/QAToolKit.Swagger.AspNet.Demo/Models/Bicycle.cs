namespace QAToolKit.Swagger.AspNet.Demo.Models
{
    public class Bicycle
    {
        /// <summary>
        /// Bike Id
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }
        /// <summary>
        /// Bike name
        /// </summary>
        /// <example>Foil</example>
        public string Name { get; set; }
        /// <summary>
        /// Bicycle brand
        /// </summary>
        /// <example>Cannondale</example>
        public string Brand { get; set; }
        /// <summary>
        /// Bicycle type
        /// </summary>
        /// <example>0</example>
        public BicycleType Type { get; set; }
    }
}
