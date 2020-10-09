using ApplicationCore;
using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using AutoMapper;
using Presentation.Admin.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presentation.Admin.Services
{
    public class EventValueFieldsService : IEventValueFieldsService
    {
        private readonly IEfRepository<ValueField> _valueFieldsRepository;
        private readonly IMapper _mapper;

        public EventValueFieldsService(IEfRepository<ValueField> valueFieldsRepository
            , IMapper mapper)
        {
            _valueFieldsRepository = valueFieldsRepository;
            _mapper = mapper;
        }

        public async Task<List<Select2Option>> GetSpeakersAsync()
        {
            var records = await _valueFieldsRepository.ListAllAsync(x => x.TypeId == ValueFieldTypes.SPEAKERS);
            return _mapper.Map<List<Select2Option>>(records);
        }

        public async Task<List<Select2Option>> GetSponsorsAsync()
        {
            var records = await _valueFieldsRepository.ListAllAsync(x => x.TypeId == ValueFieldTypes.SPONSORS);
            return _mapper.Map<List<Select2Option>>(records);
        }

        public async Task<List<int>> GetSpeakerIdsAsync(IEnumerable<Select2Option> speakers)
        {
            var speakerIds = new List<int>();

            if (speakers == null) return speakerIds;

            foreach (var speaker in speakers)
            {
                int.TryParse(speaker.Id, out int speakerId);
                if (speakerId > 0) speakerIds.Add(speakerId);
                else
                {
                    var valueField = new ValueField
                    {
                        TypeId = ValueFieldTypes.SPEAKERS,
                        Name = speaker.Text,
                        Description = speaker.Desc
                    };

                    await _valueFieldsRepository.AddAsync(valueField);

                    speakerIds.Add(valueField.Id);
                }
            }

            return speakerIds;
        }

        public async Task<List<int>> GetSponsorIdsAsync(IEnumerable<Select2Option> sponsors)
        {
            var sponsorIds = new List<int>();

            if (sponsors == null) return sponsorIds;

            foreach (var sponsor in sponsors)
            {
                int.TryParse(sponsor.Id, out int sponsorId);
                if (sponsorId > 0) sponsorIds.Add(sponsorId);
                else
                {
                    var valueField = new ValueField
                    {
                        TypeId = ValueFieldTypes.SPONSORS,
                        Name = sponsor.Text,
                        Description = sponsor.Desc
                    };

                    await _valueFieldsRepository.AddAsync(valueField);

                    sponsorIds.Add(valueField.Id);
                }
            }

            return sponsorIds;
        }
    }
}
