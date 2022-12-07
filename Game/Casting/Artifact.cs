using System;
using System.Collections.Generic;

namespace Unit04.Game.Casting
{
    /// <summary>
    /// <para>An item of cultural or historical interest.</para>
    /// <para>
    /// The responsibility of an Artifact is to provide a message about itself.
    /// </para>
    /// </summary>
    public class Artifact : Actor
    {
        private int _message = 0;
        private static int rand_x;

        /// <summary>
        /// Constructs a new instance of an Artifact.
        /// </summary>
        public Artifact()
        {
        }

        /// <summary>
        /// Gets the artifact's message.
        /// </summary>
        /// <returns>The message.</returns>
        public int GetMessage()
        {
            return _message;
        }

        /// <summary>
        /// Sets the artifact's message to the given value.
        /// </summary>
        /// <param name="message">The given message.</param>
        public void SetMessage(int message)
        {
            this._message = message;
        }

        public void GenerateObstacles(Cast cast)  
            {
                Random random = new Random();
                //set generic info for artifact
                string text = "F";
                //decide if the artifact will be a gem or a rock 
                int gemOrRock = random.Next(0,3);
                Point position = new Point(0, 0);
                position = position.Scale(15);
                rand_x = random.Next(900, 1800);
                if(gemOrRock==1){
                    text = "1";
                    Point _point1 = new Point(rand_x,500);
                    position = _point1;
                }
                else if(gemOrRock == 2){
                    text = "2";
                    Point _point2 = new Point(rand_x,400);
                    position = _point2;
                }
                else{
                    text = "0";
                    Point _point3 = new Point(rand_x,350);
                    position = _point3;
                     
             }
 
                int r = random.Next(0, 256);
                int g = random.Next(0, 256);
                int b = random.Next(0, 256);
                Color color = new Color(r, g, b);

                Actor artifact = new Actor();
                artifact.SetText(text);
                artifact.SetFontSize(15);
                artifact.SetColor(color);
                artifact.SetPosition(position);
                
                Point falling = new Point(-20, 0);
                artifact.SetVelocity(falling);
                cast.AddActor("artifacts", artifact);
            }
    }
}