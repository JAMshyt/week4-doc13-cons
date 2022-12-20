using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ПО {
	abstract class PO {
		public string Name;
		public string Company;
		public DateTime InstDate;
		public int UsePeriod;
		public int cost;

		public PO(string name, string comp, string date, int period, int cos) {
			Name = name;
			Company = comp;
			InstDate = Convert.ToDateTime(date);
			UsePeriod = period;
			cost = cos;
		}
		public virtual void Show() {
			Console.WriteLine("Имя: " + Name + "\n" +
				"Производитель: " + Company + "\n" +
				"Дата установки: " + InstDate.ToShortDateString() + "\n" +
				"Пробный период (дней): " + UsePeriod + "\n");
		}
		public virtual bool canUse() {
			if (DateTime.Now < this.InstDate.AddDays(this.UsePeriod)) return true;
			else return false;
		}
	}

	class Free : PO {
		public Free(string name, string comp) : base(name, comp, "1/1/1", 0, 0) { }

		public override void Show() {
			Console.WriteLine("Имя: " + Name + "\n" +
				"Производитель: " + Company);
		}
		public override bool canUse() {
			return true;
		}
	}

	class Shareware : PO {
		public Shareware(string name, string comp, string date, int per) : base(name, comp, date, per, 0) { }

		public override void Show() {
			base.Show();
		}
		public override bool canUse() {
			return base.canUse();
		}
	}

	class Com : PO {
		int period;
		public Com(string name, string comp, string date, int cost, int period) : base(name, comp, date, 0, cost) {
			this.period = period;
		}

		public override void Show() {
			Console.WriteLine("Имя: " + Name + "\n" +
				"Производитель: " + Company + "\n" +
				"Дата установки: " + InstDate.ToShortDateString() + "\n" +
				"Период: " + period + "\n" +
				"Цена: " + cost);
		}
		public override bool canUse() {
			if (DateTime.Now < InstDate.AddDays(period)) return true;
			else return false;
		}
	}
	internal class Program {
		static void Main(string[] args) {
			StreamReader reader = new StreamReader("..\\..\\..\\info.txt");
			string[] f = reader.ReadToEnd().Split('\n');
			string[][] arr = new string[f.Length][];
			PO[] po2 = new PO[f.Length];
			int N = 0;

			do {
				string answer = null;
				Console.WriteLine("\nВыберите действие");
				Console.WriteLine("к - ввод в консоль\tф - считать файл\tп - программы доступные на текущий день");
				answer = Console.ReadLine();
				bool flag = true;
				if (answer == "к" | answer == "ф" | answer == "п") flag = false;
				if (flag) {
					do {
						Console.Write("Введённого варианта нету\nПопробуйте ещё раз: ");
						answer = Console.ReadLine();
						if (answer == "к" | answer == "ф" | answer == "п") break;
					} while (true);
				}
				switch (answer) {
					case "к":
						Console.WriteLine("Сколько ПО вы хотите ввести?");
						do {
							try {
								N = Convert.ToInt32(Console.ReadLine());
								break;
							}
							catch { Console.WriteLine("Error"); }
						} while (true);
						PO[] p = new PO[N];
						for (int i = 0; i < N; i++) {
							Console.WriteLine("Какое ПО вы вводите?\nб - Бесплатное\tу - Условно-бесплатное\tк - Комерческое");
							string answer2 = Console.ReadLine();
							bool flag2 = true;
							if (answer2 == "б" | answer2 == "у" | answer2 == "к") flag2 = false;
							if (flag2) {
								do {
									Console.Write("Введённого варианта нету\nПопробуйте ещё раз: ");
									answer2 = Console.ReadLine();
									if (answer2 == "б" | answer2 == "у" | answer2 == "к") break;
								} while (true);
							}
							switch (answer2) {
								case "б":
									Console.WriteLine("\nВведите имя: ");
									string Name = Console.ReadLine();
									Console.WriteLine("Введите компанию: ");
									string Company = Console.ReadLine();
									p[i] = new Free(Name, Company);
									Console.WriteLine("\n");
									break;
								case "у":
									Console.WriteLine("Введите имя: ");
									Name = Console.ReadLine();
									Console.WriteLine("Введите компанию: ");
									Company = Console.ReadLine();
									DateTime InstDate;
									int UsePeriod;
									int cost;
									do {
										try {
											Console.WriteLine("Введите дату установки: ");
											InstDate = Convert.ToDateTime(Console.ReadLine());
											Console.WriteLine("Введите пробный период (дней): ");
											UsePeriod = Convert.ToInt32(Console.ReadLine());
											Console.WriteLine("Введите цену: ");
											cost = Convert.ToInt32(Console.ReadLine());
											break;
										}
										catch { Console.WriteLine("Ошибка"); }
									} while (true);
									p[i] = new Shareware(Name, Company, Convert.ToString(InstDate), UsePeriod);
									Console.WriteLine("\n");
									break;

								case "к":
									int period;
									Console.WriteLine("Введите имя: ");
									Name = Console.ReadLine();
									Console.WriteLine("Введите компанию: ");
									Company = Console.ReadLine();
									do {
										try {
											Console.WriteLine("Введите дату установки: ");
											InstDate = Convert.ToDateTime(Console.ReadLine());
											Console.WriteLine("Введите период (дней): ");
											period = Convert.ToInt32(Console.ReadLine());
											Console.WriteLine("Введите цену: ");
											cost = Convert.ToInt32(Console.ReadLine());
											break;
										}
										catch { Console.WriteLine("Ошибка"); }
									} while (true);
									p[i] = new Com(Name, Company, Convert.ToString(InstDate), cost, period);
									Console.WriteLine("\n");
									break;
							}

						}
						Console.WriteLine("\n Список всех ПО:");
						foreach (PO e in p) {
							Console.WriteLine("_____________");
							e.Show();
							Console.Write("_____________");
						}

						Console.WriteLine("\n");
						break;

					case "ф":
						for (int i = 0; i < f.Length; i++) {
							arr[i] = f[i].Split(',');
						}
						for (int i = 0; i < arr.Length; i++) {
							for (int j = 0; j < arr[i].Length; j++) {
								if (arr[i].Length == 2) {
									po2[i] = new Free(arr[i][0], arr[i][1]);
									i++;
								}
								if (arr[i].Length == 4) {
									po2[i] = new Shareware(arr[i][0], arr[i][1], arr[i][2], Convert.ToInt32(arr[i][3]));
								}
								if (arr[i].Length == 5) {
									po2[i] = new Com(arr[i][0], arr[i][1], arr[i][2], Convert.ToInt32(arr[i][4]), Convert.ToInt32(arr[i][3]));
								}
							}
						}
						try {
							Console.WriteLine("\n Список всех ПО:");
							foreach (PO e in po2) {
								Console.WriteLine("_____________");
								e.Show();
								Console.Write("_____________");
							}
						}
						catch { Console.WriteLine("Пусто"); }

						Console.WriteLine("\n");
						break;
					case "п":
						for (int i = 0; i < f.Length; i++) {
							arr[i] = f[i].Split(',');
						}
						for (int i = 0; i < arr.Length; i++) {
							for (int j = 0; j < arr[i].Length; j++) {
								if (arr[i].Length == 2) {
									po2[i] = new Free(arr[i][0], arr[i][1]);
									i++;
								}
								if (arr[i].Length == 4) {
									po2[i] = new Shareware(arr[i][0], arr[i][1], arr[i][2], Convert.ToInt32(arr[i][3]));
								}
								if (arr[i].Length == 5) {
									po2[i] = new Com(arr[i][0], arr[i][1], arr[i][2], Convert.ToInt32(arr[i][4]), Convert.ToInt32(arr[i][3]));
								}
							}
						}
						try {
							foreach (PO e in po2) {
								if (e.canUse()) {
									Console.WriteLine("_____________");
									e.Show();
									Console.Write("_____________");
								}
							}
						}
						catch {
							Console.WriteLine("Пусто");
						}
						Console.WriteLine("\n");
						break;
				}
			} while (true);
		}
	}
}
