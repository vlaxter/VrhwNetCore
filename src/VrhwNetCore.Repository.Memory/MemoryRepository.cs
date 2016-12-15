using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VrhwNetCore.Shared.Dtos;
using VrhwNetCore.Shared.Interfaces;

namespace VrhwNetCore.Repository.Memory
{
    public class MemoryRepository : IDiffRepository
    {
        private static Dictionary<int, DiffDto> _memory = new Dictionary<int, DiffDto>();

        public DiffDto UpsertDiff(int id, string left, string right)
        {
            if (!_memory.ContainsKey(id))
            {
                _memory[id] = new DiffDto();
            }

            _memory[id].Left = left != null ? left : _memory[id].Left;
            _memory[id].Right = right != null ? right : _memory[id].Right;

            return _memory[id];
        }

        public DiffDto GetDiff(int id)
        {
            if (!_memory.ContainsKey(id))
            {
                return null;
            }

            return _memory[id];
        }
    }
}
