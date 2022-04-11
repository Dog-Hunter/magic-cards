using System;
using System.Collections.Generic;

namespace magicCards
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Game game = new Game();
            game.game();

        }
    }

    internal class Card
    {
        public int score;
        public Card(int score)
        {
            this.score = score;
        }
    }

    internal class Player
    {
        public int manna = 1;
        public int health = 30;
        public string name;
        public List<Card> cards = new List<Card>();
        public Card choosenCard;

        public Player(string name)
        {
            this.name = name;
        }
        public void AddManna()
        {
            // This func add manna to player
            // until it's become more 10
            if (manna <= 10)
                manna++;
        }
        public void CheckCard()
        {
            // This func check number of cards
            // if player doesn't have any cars
            // he will get damage
            if (cards.Count == 0)
                health--;
            else if (cards.Count == 5)
                //if got 5 card, it's must be deleted
                cards.Remove(cards[cards.Count - 1]);
        }
        public void ChooseCard()
        {
            // Allows player choose car to play
            while (true)
            {
                Console.WriteLine("Choose Your card:");
                for (int i = 0; i < cards.Count; i++)
                    Console.WriteLine($"Card {i + 1}, Attack {cards[i].score}");
                var choice = int.Parse(Console.ReadLine());
                if (manna >= cards[choice].score)
                {
                    choosenCard = cards[choice - 1];
                    break;
                }
                else
                    Console.WriteLine("You don't have enoght manna");
            }
        }
    }

    internal class Game
    {
        Random random = new Random();
        private List<int[]> cards_type = new List<int[]>()
        {
            new int[] {2, 0},
            new int[] {2, 1},
            new int[] {3, 2},
            new int[] {4, 3},
            new int[] {3, 4},
            new int[] {2, 5},
            new int[] {2, 6},
            new int[] {1, 7},
            new int[] {1, 8}
        };
        List<Card> cards = new List<Card>();
        Player player1 = new Player("John");
        Player player2 = new Player("Tyler");
        void GenerateCards()
        {
            // Generate cards according the Dictionary
            foreach (var item in cards_type)
            {
                for (int i = 0; i < item[0]; i++)
                {
                    cards.Add(new Card(item[1]));
                }
            }
            for (int i = cards.Count - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);
                var temp = cards[j];
                cards[j] = cards[i];
                cards[i] = temp;
            }
        }
        void Distribution()
        {
            // Distribute cards between players
            for (int i = 0; i < 6; i += 2)
            {
                player1.cards.Add(cards[i]);
                player2.cards.Add(cards[i + 1]);
                cards.Remove(cards[i]);
                cards.Remove(cards[i + 1]);
            }
        }
        string TakeCard(Player player)
        {
            Random random = new Random();
            var index = random.Next(cards.Count);
            player.cards.Add(cards[index]);
            string card = $"{cards[index]}";
            cards.Remove(cards[index]);
            return card;
        }
        void PlayerDuty(Player player)
        {
            player.AddManna();
            Console.WriteLine($"It's {player.name} turn!");
            Console.WriteLine("Look what you got:");
            Console.WriteLine($"Manna: {player.manna}");
            Console.WriteLine($"Health: {player.health}");
            Console.WriteLine($"You get this card: {TakeCard(player)}");
            player.CheckCard();
            Console.WriteLine("Let's card to play!");
            player.ChooseCard();
            Console.Clear();
        }

        public void game()
        {
            GenerateCards();
            Distribution();
            while (true)
            {
                PlayerDuty(player1);
                PlayerDuty(player2);
                CheckAttack(player1, player2);
                CheckAttack(player2, player1);
            }
        }
        void CheckAttack(Player p1, Player p2)
        {
            if (!(p1.choosenCard is null))
            {
                p2.health -= p1.choosenCard.score;
            }
            Console.WriteLine(CheckWinner(p1, p2));
        }
        string CheckWinner(Player p1, Player p2)
        {
            if (p1.health <= 0)
                return $"Winner is {p2.name}";
            else if (p2.health <= 0)
                return $"Winner is {p1.name}";
            else
                return "Next Round";
        }
    }
}
