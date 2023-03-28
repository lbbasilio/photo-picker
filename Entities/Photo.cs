using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS8618 // Suppress "Non-nullable field must contain a non-null value when exiting constructor." warning
namespace PhotoPicker.Entities
{
    public class Photo
    {
        public long Id { get; set; }
        public DateTime UploadDate { get; set; }
        public PhotoState State { get; set; }
        public MediaType MediaType { get; set; }
        public DateTime? ExtractedDate { get; set; }
        public string S3Url { get; set; }
        public long UploaderId { get; set; }
        public long? RecognizedUserId { get; set; }
        public int? AlogrithmConfidence { get; set; }
        public long? SelectedUserId { get; set; }

        #region Navigation Properties
        [ForeignKey("UploaderId")]
        public virtual User? Uploader { get; set; }
        [ForeignKey("RecognizedUserId")]
        public virtual User? RecognizedUser { get; set; }
        [ForeignKey("SelectedUserId")]
        public virtual User? SelectedUser { get; set; }
        #endregion
    }

    public enum PhotoState
    {
        ProcessingPending,
        NoFacesFound,
        NoMatchesFound,
        MatchedOne,
        MatchedMultiple,
        UploaderConfirmed
    }

    public enum MediaType
    {
        Photo,
        Video
    }
}
#pragma warning restore CS8618