using Domain.Enums;

namespace Application.Helper
{
    public class State
    {
        private List<(long userTelegramId, EUserState? state, int? categoryId)> values;
        public State()
        {
            values = new List<(long userTelegramId, EUserState? state, int? categoryId)>();
        }

        public void SetState(long userTelegramId, EUserState? state, int? categoryId = null)
        {
            var index = values.FindIndex(x => x.userTelegramId == userTelegramId);
            if (index == -1)
            {
                values.Add((userTelegramId, state, categoryId));
            }
            else
            {
                values[index] = (userTelegramId, state, categoryId);
            }
        }

        public (EUserState? state, int? categoryId) GetState(long userTelegramId)
        {
            var index = values.FindIndex(x => x.userTelegramId == userTelegramId);
            if (index == -1)
            {
                return (null, null);
            }

            return (values[index].state, values[index].categoryId);
        }
    }
}
