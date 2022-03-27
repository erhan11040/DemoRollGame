using DemoRollGame.Models.Configuration;
using DemoRollGame.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoRollGame.DbCore.Factory
{
    public class ContextFactory : IContextFactory
    {
        private readonly IOptions<ConnectionSettings> _connectionOptions;

        public ContextFactory(IOptions<ConnectionSettings> connectionOptions)
        {
            _connectionOptions = connectionOptions;
        }

        public demo_roll_gameContext DbContext => new demo_roll_gameContext(GetDataContext().Options);

        private DbContextOptionsBuilder<demo_roll_gameContext> GetDataContext()
        {
            ValidateDefaultConnection();
            var optionsBuilder = new DbContextOptionsBuilder<demo_roll_gameContext>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=demo_roll_game;Trusted_Connection=True;");

            return optionsBuilder;
        }

        private void ValidateDefaultConnection()
        {
            if (string.IsNullOrEmpty(_connectionOptions.Value.DefaultConnection))
            {
                throw new ArgumentNullException(nameof(_connectionOptions.Value.DefaultConnection));
            }
        }
    }
}
