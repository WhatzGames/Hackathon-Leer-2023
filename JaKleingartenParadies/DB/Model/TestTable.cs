namespace JaKleingartenParadies.DB;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table("TestTable")]
public class TestTable
{
    public long Uid { get; set; }
    public string Test1 { get; set; }
    public string Test2 { get; set; }
    public string Test3 { get; set; }
    public string Test4 { get; set; }
    public string Test5 { get; set; }
}