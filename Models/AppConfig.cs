using System.ComponentModel.DataAnnotations;

namespace PMChecklist_PD_API.Models
{
    public class AppConfig
    {
        [Required(ErrorMessage = "The app id is required.")]
        public string? AppID { get; set; }
        [Required(ErrorMessage = "The app display is required.")]
        public string? AppName { get; set; }
        [Required(ErrorMessage = "The group machine display is required.")]
        public string? GroupMachine { get; set; }
        [Required(ErrorMessage = "The prefix group machine is required.")]
        public string? PF_GroupMachine { get; set; }
        [Required(ErrorMessage = "The machine display is required.")]
        public string? Machine { get; set; }
        [Required(ErrorMessage = "The prefix machine is required.")]
        public string? PF_Machine { get; set; }
        [Required(ErrorMessage = "The check list display is required.")]
        public string? CheckList { get; set; }
        [Required(ErrorMessage = "The prefix check list is required.")]
        public string? PF_CheckList { get; set; }
        [Required(ErrorMessage = "The group check list display is required.")]
        public string? GroupCheckList { get; set; }
        [Required(ErrorMessage = "The prefix group check list is required.")]
        public string? PF_GroupCheckList { get; set; }
        [Required(ErrorMessage = "The check list option display is required.")]
        public string? CheckListOption { get; set; }
        [Required(ErrorMessage = "The prefix check list option is required.")]
        public string? PF_CheckListOption { get; set; }
        [Required(ErrorMessage = "The match check list option display is required.")]
        public string? MatchCheckListOption { get; set; }
        [Required(ErrorMessage = "The prefix match check list option is required.")]
        public string? PF_MatchCheckListOption { get; set; }
        [Required(ErrorMessage = "The match form machine display is required.")]
        public string? MatchFormMachine { get; set; }
        [Required(ErrorMessage = "The prefix match form machine is required.")]
        public string? PF_MatchFormMachine { get; set; }
        [Required(ErrorMessage = "The form display is required.")]
        public string? Form { get; set; }
        [Required(ErrorMessage = "The prefix form is required.")]
        public string? PF_Form { get; set; }
        [Required(ErrorMessage = "The subform display is required.")]
        public string? SubForm { get; set; }
        [Required(ErrorMessage = "The prefix subform is required.")]
        public string? PF_SubForm { get; set; }
        [Required(ErrorMessage = "The expected result display is required.")]
        public string? ExpectedResult { get; set; }
        [Required(ErrorMessage = "The prefix expected result is required.")]
        public string? PF_ExpectedResult { get; set; }
        [Required(ErrorMessage = "The user permission display is required.")]
        public string? UsersPermission { get; set; }
        [Required(ErrorMessage = "The prefix user permission is required.")]
        public string? PF_UsersPermission { get; set; }
        [Required(ErrorMessage = "The time schedule display is required.")]
        public string? TimeSchedule { get; set; }
        [Required(ErrorMessage = "The prefix time schedule is required.")]
        public string? PF_TimeSchedule { get; set; }
    }
}
