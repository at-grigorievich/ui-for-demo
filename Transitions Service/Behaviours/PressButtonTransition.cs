using System;
using ATG.DTO;

namespace ATG.Transition
{
    public sealed class PressButtonTransition : ITransitionBehaviour<GraphicButtonDTO>
    {
        private readonly PressButtonTransitionData _config;

        private GraphicButtonDTO? _lastDTO;

        public PressButtonTransition(PressButtonTransitionData config)
        {
            _config = config;
        }

        public void Execute(GraphicButtonDTO slave, Action callback = null)
        {
            if (slave.IsPressed == true)
            {
                slave.MainRect.SetTop(_config.MainRectPressTopBottom.x);
                slave.MainRect.SetBottom(_config.MainRectPressTopBottom.y);

                slave.BackgroundRect.SetTop(_config.BackgorundPressTopBottom.x);
                slave.BackgroundRect.SetBottom(_config.BackgorundPressTopBottom.y);

                slave.ContentRect.anchoredPosition = _config.ContentPressTopBottom;
            }
            else
            {
                slave.MainRect.SetTop(_config.MainRectDefaultTopBottom.x);
                slave.MainRect.SetBottom(_config.MainRectDefaultTopBottom.y);

                slave.BackgroundRect.SetTop(_config.BackgorundDefaultTopBottom.x);
                slave.BackgroundRect.SetBottom(_config.BackgorundDefaultTopBottom.y);

                slave.ContentRect.anchoredPosition = _config.ContentDefaultTopBottom;
            }
            _lastDTO = slave;

        }

        public void Dispose()
        {
            if (_lastDTO.HasValue == false) return;

            var btn = _lastDTO.Value;

            btn.MainRect.SetTop(_config.MainRectDefaultTopBottom.x);
            btn.MainRect.SetBottom(_config.MainRectDefaultTopBottom.y);

            btn.BackgroundRect.SetTop(_config.BackgorundDefaultTopBottom.x);
            btn.BackgroundRect.SetBottom(_config.BackgorundDefaultTopBottom.y);

            btn.ContentRect.anchoredPosition = _config.ContentDefaultTopBottom;

            _lastDTO = null;
        }
    }
}