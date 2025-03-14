using Godot;
using Game.Bus;

namespace Game.Pellets
{

	public class PowerPellet : Pellet
	{

		private bool _isActive = true;

		public override void ResetPellet()
		{
			VisualComponent.Visible = true;
			CollisionComponent.SetDeferred("disabled", false);
			_isActive = true;
		}


		public void OnAreaEntered(Area2D area)
		{
			if (area.IsInGroup(PlayerNodeGroup))
			{
				CollectPellet();
				PelletEventBus.Instance.EmitSignal("PowerPelletCollected");
				_isActive = false;
			}
		}


		public void OnTimerTimeout()
		{
			if (_isActive)
			{
				VisualComponent.Visible = !VisualComponent.Visible;
			}
		}
	}
}
