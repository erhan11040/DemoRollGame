using DemoRollGame.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoRollGame.DbCore.Factory
{
    public interface IContextFactory
    {
        demo_roll_gameContext DbContext { get; }
    }
}
