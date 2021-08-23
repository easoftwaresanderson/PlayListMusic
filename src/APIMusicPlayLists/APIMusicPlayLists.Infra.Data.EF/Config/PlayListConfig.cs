using APIMusicPlayLists.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIMusicPlayLists.Infra.Data.EF.Config
{
    public class PlayListConfig : IEntityTypeConfiguration<PlayList>
    {
        public void Configure(EntityTypeBuilder<PlayList> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasOne(p => p.Device)
                .WithOne(d => d.PlayList)
                .HasForeignKey<Device>(e => e.PlayListID);

            builder
                  .HasMany(m => m.Musics)
                  .WithOne(p => p.PlayList)
                  .HasForeignKey(m => m.PlayListID);

        }
    }
}
