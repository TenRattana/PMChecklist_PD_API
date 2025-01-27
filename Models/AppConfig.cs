using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class AppConfig
    {
        public string? AppID { get; set; } 
        public string? AppName { get; set; }  
        public string? GroupMachine { get; set; }  
        public string? PF_GroupMachine { get; set; } 
        public string? Machine { get; set; } 
        public string? PF_Machine { get; set; }  
        public string? CheckList { get; set; }  
        public string? PF_CheckList { get; set; } 
        public string? GroupCheckList { get; set; } 
        public string? PF_GroupCheckList { get; set; }  
        public string? CheckListOption { get; set; } 
        public string? PF_CheckListOption { get; set; }  
        public string? MatchCheckListOption { get; set; }  
        public string? PF_MatchCheckListOption { get; set; } 
        public string? MatchFormMachine { get; set; }  
        public string? PF_MatchFormMachine { get; set; } 
        public string? Form { get; set; }  
        public string? PF_Form { get; set; }
        public string? SubForm { get; set; } 
        public string? PF_SubForm { get; set; } 
        public string? ExpectedResult { get; set; }  
        public string? PF_ExpectedResult { get; set; } 
        public string? UsersPermission { get; set; }  
        public string? PF_UsersPermission { get; set; }  
        public string? TimeSchedule { get; set; } 
        public string? PF_TimeSchedule { get; set; }  
    }
}
