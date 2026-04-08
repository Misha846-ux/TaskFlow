using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using TaskFlow.Application.DTOs.ChangeDTOs;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.Services
{
    public class ChangeService : IChangeService
    {
        private readonly IChangeRepository _changeRepository;
        private readonly IMapper _mapper;

        public ChangeService(IChangeRepository changeRepository, IMapper mapper)
        {
            _changeRepository = changeRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<ChangeDto>> GetChangesByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            var entities = await _changeRepository.GetChangesByUserIdAsync(userId, cancellationToken);
            return _mapper.Map<ICollection<ChangeDto>>(entities);
        }

        public async Task<ICollection<ChangeDto>> GetUnreadChangesByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            return await GetChangesByUserIdAsync(userId, cancellationToken);
        }

        public async Task<ICollection<ChangeDto>> GetChangesByUserIdPaginatedAsync(int userId, int count, int side, CancellationToken cancellationToken)
        {
            var entities = await _changeRepository.GetChangesByUserIdPaginatedAsync(userId, count, side, cancellationToken);
            return _mapper.Map<ICollection<ChangeDto>>(entities);
        }

        public async Task CreateChangeAsync(ChangeTableType table, int noteId, ChangeType changeType, List<int> userIds, CancellationToken cancellationToken)
        {
            var changes = new List<ChangeEntity>();
            string message = (table, changeType) switch
            {
                (ChangeTableType.Projects, ChangeType.Created) => $"Project #{noteId} was successfully created",
                (ChangeTableType.Projects, ChangeType.Updated) => $"Project #{noteId} was updated",
                (ChangeTableType.Projects, ChangeType.Deleted) => $"Project #{noteId} was deleted",
                (ChangeTableType.Tasks, ChangeType.Created) => $"Task #{noteId} was successfully created",
                (ChangeTableType.Tasks, ChangeType.Updated) => $"Task #{noteId} was updated",
                (ChangeTableType.Tasks, ChangeType.Deleted) => $"Task #{noteId} was deleted",
                (ChangeTableType.Companies, ChangeType.Created) => $"Company #{noteId} was successfully created",
                (ChangeTableType.Companies, ChangeType.Updated) => $"Company #{noteId} was updated",
                (ChangeTableType.Companies, ChangeType.Deleted) => $"Company #{noteId} was deleted",
                _ => $"Entity #{noteId} was {changeType.ToString().ToLower()}"
            };

            foreach (var userId in userIds)
            {
                var change = new ChangeEntity
                {
                    Table = table,
                    NoteId = noteId,
                    UserId = userId,
                    ChangeType = changeType,
                    CreatedAt = DateTime.UtcNow,
                    Message = message
                };
                changes.Add(change);
            }
            await _changeRepository.CreateChangesAsync(changes, cancellationToken);
        }
    }
}