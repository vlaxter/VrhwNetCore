using System;
using System.Collections.Generic;
using System.Text;
using VrhwNetCore.Shared.Helpers;
using VrhwNetCore.Shared.Interfaces;
using VrhwNetCore.Shared.Models;

namespace VrhwNetCore.Core.Services
{
    public class DiffService : IDiffService
    {
        private readonly IDiffRepository _diffReository;

        public DiffService(IDiffRepository diffRepository)
        {
            _diffReository = diffRepository;
        }

        public DiffModel Left(int id, string data)
        {
            if (data.IsBase64String())
            {
                var diffDto = _diffReository.UpsertDiff(id, data, null);
                return new DiffModel
                {
                    Left = diffDto.Left,
                    Right = diffDto.Right
                };
            }

            return null;
        }

        public DiffModel Right(int id, string data)
        {
            if (data.IsBase64String())
            {
                var diffDto = _diffReository.UpsertDiff(id, null, data);
                return new DiffModel
                {
                    Left = diffDto.Left,
                    Right = diffDto.Right
                };
            }

            return null;
        }

        public object GetDiff(int id)
        {
            var diff = _diffReository.GetDiff(id);

            if (diff == null)
            {
                return null;
            }

            if (diff.Right == null || diff.Left == null)
            {
                return new
                {
                    diffResultType = DiffMessages.SizeDoNotMatchResponse
                };
            }

            var leftData = Convert.FromBase64String(diff.Left);
            var decodedLeftString = Encoding.UTF8.GetString(leftData);
            var rightData = Convert.FromBase64String(diff.Right);
            var decodedRightString = Encoding.UTF8.GetString(rightData);

            if (decodedLeftString == decodedRightString)
            {
                return new
                {
                    diffResultType = DiffMessages.EqualsResponse
                };
            }
            else if (decodedLeftString.Length != decodedRightString.Length)
            {
                return new
                {
                    diffResultType = DiffMessages.SizeDoNotMatchResponse
                };
            }

            var leftPieces = decodedLeftString.ToCharArray();
            var rightPieces = decodedRightString.ToCharArray();

            var differences = new List<DiffSection>();
            DiffSection currentDiff = null;

            for (int i = 0; i < leftPieces.Length; i++)
            {
                if (currentDiff == null)
                {
                    if (leftPieces[i] != rightPieces[i])
                    {
                        currentDiff = new DiffSection
                        {
                            Offset = i,
                            Length = 1
                        };
                    }
                }
                else
                {
                    if (leftPieces[i] != rightPieces[i])
                    {
                        currentDiff.Length++;
                    }
                    else
                    {
                        differences.Add(currentDiff);
                        currentDiff = null;
                    }
                }
            }

            if (currentDiff != null)
            {
                differences.Add(currentDiff);
            }

            return new
            {
                diffResultType = DiffMessages.ContentDoNotMatchResponse,
                diffs = differences
            };
        }
    }
}