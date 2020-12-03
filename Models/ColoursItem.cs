//using Microsoft.Azure.Cosmos.Table;

namespace ColoursAPI.Models
{
    public class ColoursItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

    //public class ColoursItemEntity : TableEntity
    //{

    //    public ColoursItemEntity()
    //    {
    //    }
    //    public ColoursItemEntity(string Thingid)
    //    {
    //        PartitionKey = "Thing";
    //        RowKey = Thingid;
    //    }

    //    public int Id { get; set; }
    //    public string Name { get; set; }

    //}

}
