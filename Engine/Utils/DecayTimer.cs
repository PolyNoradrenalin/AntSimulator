using System;
 using System.Timers;
 
 namespace AntEngine.Entities
 {
     /// <summary>
     /// Allows for the use of a timer that decays over time.
     /// </summary>
     public class DecayTimer
     {
         protected DateTime creationTime = DateTime.Now;
         
         protected TimeSpan _lifeSpan;

         public DecayTimer(TimeSpan maxLifeSpan)
         {
             MaxLifeSpan = maxLifeSpan;
         }

         /// <summary>
         /// The timer has decayed event.
         /// </summary>
         public event EventHandler TimerDecayed;
 
         /// <summary>
         /// Current LifeSpan (in seconds).
         /// </summary>
         public TimeSpan LifeSpan
         {
             get => _lifeSpan;
             protected set => _lifeSpan = value > MaxLifeSpan ? MaxLifeSpan : value;
         }
         
         /// <summary>
         /// Maximum LifeSpan of the pheromone.
         /// </summary>
         public TimeSpan MaxLifeSpan { get; set; }

         public void Update()
         {
             LifeSpan = creationTime - DateTime.Now;
             if (LifeSpan == MaxLifeSpan)
             {
                 OnDecay(EventArgs.Empty);
             }
         }

         protected void OnDecay(EventArgs e)
         {
             TimerDecayed?.Invoke(this, e);
         }
     }
 }