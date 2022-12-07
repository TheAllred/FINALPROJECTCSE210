using System;
using System.Collections.Generic;
using Unit04.Game.Casting;
using Unit04.Game.Services;


namespace Unit04.Game.Directing
{
    /// <summary>
    /// <para>A person who directs the game.</para>
    /// <para>
    /// The responsibility of a Director is to control the sequence of play.
    /// </para>
    /// </summary>
    public class Director
    {
         
         private static int CELL_SIZE = 15;
        private static int COLS = 60;
        private static int ROWS = 40;
         private static int MAX_X = 900;
        private static int MAX_Y = 600;
        private KeyboardService _keyboardService = null;
        private VideoService _videoService = null;
        private Random random = new Random();
        private Point stopped = new Point(0, 0);
        private Point bottom = new Point(100, 500);

        private Point gravity = new Point(0,0);
        private Point gravityCONST = new Point(0,9);

        /// <summary>
        /// Constructs a new instance of Director using the given KeyboardService and VideoService.
        /// </summary>
        /// <param name="keyboardService">The given KeyboardService.</param>
        /// <param name="videoService">The given VideoService.</param>
        public Director(KeyboardService keyboardService, VideoService videoService)
        {
            this._keyboardService = keyboardService;
            this._videoService = videoService;

        }

        /// <summary>
        /// Starts the game by running the main game loop for the given cast.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        public void StartGame(Cast cast)
        {
            _videoService.OpenWindow();
            while (_videoService.IsWindowOpen())
            {
                GetInputs(cast);
                DoUpdates(cast);
                DoOutputs(cast);
            }
            _videoService.CloseWindow();
        }

        /// <summary>
        /// Gets directional input from the keyboard and applies it to the robot.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        private void GetInputs(Cast cast)
        {
            Actor robot = cast.GetFirstActor("robot");
            
            if (robot.GetPosition().GetY()>480){
            gravity = gravity.Add(_keyboardService.GetDirection());}
            if (robot.GetPosition().GetY()<480){
            gravity = gravity.Add(gravityCONST);
            }
            if (robot.GetPosition().GetY()>485){
                gravity = stopped.Add(_keyboardService.GetDirection());
            }
            robot.SetVelocity(gravity);     
        }

        /// <summary>
        /// Updates the robot's position and resolves any collisions with artifacts.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        private void DoUpdates(Cast cast)
        {
            Actor banner = cast.GetFirstActor("banner");
            Actor robot = cast.GetFirstActor("robot");
            List<Actor> artifacts = cast.GetActors("artifacts");

            // banner.SetText(banner.getValue().ToString());
            // banner.SetText(robot.GetPosition().GetY().ToString());
            banner.SetText(gravity.GetY().ToString());
            int maxX = _videoService.GetWidth();
            int maxY = _videoService.GetHeight();
            robot.MoveNext(maxX, maxY);

            //ADD BASE LINE THAT ROBOT CANT GO BELOW 
             if (robot.GetPosition().GetY() > 500 ){
                    robot.SetPosition(bottom);
                }


            foreach (Actor actor in artifacts)
            {
                if (robot.GetPosition().Equals(actor.GetPosition()))
                {
                    Artifact artifact = (Artifact) actor;
                    int message = artifact.GetMessage();
                    // banner.addValue(message);
                    // banner.SetText(banner.getValue().ToString());
                    
                        int x = random.Next(1, COLS);
                    
                        int y = 0;
                        Point position = new Point(x, y);
                        // position = position.Scale(CELL_SIZE);
                    artifact.SetPosition(position);
                }
                actor.MoveNext(maxX, maxY);
               
            } 
        }

        /// <summary>
        /// Draws the actors on the screen.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        public void DoOutputs(Cast cast)
        {
            List<Actor> actors = cast.GetAllActors();
            _videoService.ClearBuffer();
            _videoService.DrawActors(actors);
            _videoService.FlushBuffer();
        }

    }
}