using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SecretSanta.Business.Dto
{
    public interface IEntity
    {
        [Required]
        int Id { get; }
    }
}
