using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class MatchCheckList
    {
        public int MCListID { get; set; } 
        public string? CListID { get; set; }  
        public string? GCLOptionID { get; set; }  
        public string? CTypeID { get; set; }  
        public string? DTypeID { get; set; } 
        public byte? DTypeValue { get; set; }
        public string? SFormID { get; set; }  
        public byte? Rowcolumn { get; set; }
        public bool? Important { get; set; }
        public bool Required { get; set; } 
        public double? MinLength { get; set; }
        public double? MaxLength { get; set; }
        public string? Description { get; set; }
        public string? Placeholder { get; set; }
        public string? Hint { get; set; }
        public byte DisplayOrder { get; set; }  

        public CheckLists? CheckList { get; set; } 
        public GroupCheckListOptions? GroupCheckListOption { get; set; }  
        public CheckListTypes? CheckListType { get; set; }  
        public DataTypes? DataType { get; set; } 
        public SubForms? SubForm { get; set; } 
    }
}
