using System;
using System.Collections.Generic;
using System.Linq;
using NEC.TEAM.WebAPI.Models.Base;
using Newtonsoft.Json;

namespace NEC.TEAM.WebAPI.Models
{
    public class DigitalCaseDto : Dto
    {
        #region Properties.

        private string _caseId;
        [JsonProperty("caseId")]
        public string CaseId
        {
            get { return _caseId; }
            set
            {
                _caseId = value;
                RaisePropertyChanged(nameof(this.CaseId));
            }
        }

        private string _caseTitle;
        [JsonProperty("caseTitle")]
        public string CaseTitle
        {
            get { return _caseTitle; }
            set
            {
                _caseTitle = value;
                RaisePropertyChanged(nameof(this.CaseTitle));
            }
        }

        private int _caseNumber;
        [JsonProperty("caseNumber")]
        public int CaseNumber
        {
            get { return _caseNumber; }
            set
            {
                _caseNumber = value;
                RaisePropertyChanged(nameof(this.CaseNumber));
            }
        }

        private List<MediaDto> _documentList;
        [JsonProperty("listDocumentDto")]
        public List<MediaDto> DocumentList
        {
            get { return _documentList; }
            set
            {
                _documentList = value;
                RaisePropertyChanged(nameof(this.DocumentList));
            }
        }

        private List<DigitalCaseDto> _linkedCases;
        [JsonProperty("linkedCases")]
        public List<DigitalCaseDto> LinkedCases
        {
            get { return _linkedCases; }
            set
            {
                _linkedCases = value;
                RaisePropertyChanged(nameof(this.LinkedCases));
            }
        }

        private List<LinkedMediaDto> _linkedMediaList;
        [JsonProperty("linkedMedia")]
        public List<LinkedMediaDto> LinkedMediaList
        {
            get { return _linkedMediaList; }
            set
            {
                _linkedMediaList = value;
                RaisePropertyChanged(nameof(this.LinkedMediaList));
            }
        }


        #endregion

        #region Constructor.

        public DigitalCaseDto() : base()
        {
            this.DocumentList = new List<MediaDto>();
            this.LinkedCases = new List<DigitalCaseDto>();
            this.LinkedMediaList = new List<LinkedMediaDto>();
        }

        #endregion

        #region Methods.

        public void AddMedia(MediaDto media)
        {
            media.CaseId = this.CaseId;
            this.DocumentList.Add(media);
        }

        public void LinkToCase(DigitalCaseDto digitalCase)
        {
            if (digitalCase == null || String.IsNullOrEmpty(digitalCase.CaseId))
            {
                throw new Exception("Linked Case or Case ID cannot be null");
            }
            if (digitalCase.CaseId == this.CaseId)
            {
                throw new Exception("Cannot link to the same case");
            }
            this.LinkedCases.Add(digitalCase);
        }

        public void LinkMedia(string relation, string sourceMediaContentId, MediaDto linkedMedia)
        {
            var sourceMedia = this.DocumentList.FirstOrDefault(x => x.ContentId == sourceMediaContentId);
            if (sourceMedia == null)
            {
                throw new Exception("Cannot find the source media");
            }
            if (linkedMedia == null || linkedMedia.ContentId == null || linkedMedia.ContentId == sourceMediaContentId)
            {
                throw new Exception("Invalid linked media");
            }
            LinkedMediaDto lm = new LinkedMediaDto(relation, sourceMedia, linkedMedia);
            this.LinkedMediaList.Add(lm);
        }

        public void LinkMedia(string relation, MediaDto sourceMedia, MediaDto linkedMedia, Direction direction = Direction.Bi)
        {
            if (sourceMedia == null)
            {
                throw new Exception("Source Media cannot be null.");
            }
            if (!this.DocumentList.Contains(sourceMedia))
            {
                throw new Exception("Cannot find the source media");
            }
            if (linkedMedia == null)
            {
                throw new Exception("Cannot find the linked media");
            }
            LinkedMediaDto lm = new LinkedMediaDto(relation, sourceMedia, linkedMedia);
            lm.Direction = direction;
            this.LinkedMediaList.Add(lm);
        }

        #endregion
    }
}