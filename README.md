# Faculty Wars
Game development project. KPI 2019

Разработчики: Златокрылец Николай, Гасс Леонид

Билд игры под Windows находится в папке Builds/Build_v1.6

## Общие положения
Faculty wars - карточная игра, имеющая элементы настольной игры.
Матчи проходят для двух игроков (на данный момент реализован режим игры человека против компьютера).
Цель игры - победить все фишки противника.

### Схожие проекты

* **Hearthstone.** Популярная коллекционная карточная игра. Имеет схожие механики разыгрывания существ и применения заклинаний.

* **Gwent.** Коллекционная карточная игра. Помимо прочего имеет усложненную структуру поля игры.

* **Отличия нашей игры.** Существа представляют из себя не карты, а отдельные сущности. Поле также представляет из себя сложную структуру, которая влияет на силы персонажей и с которой можно взаимодействовать.

### Наш игрок
Человек, который любит играть в карточные игры, разрабатывать собственную стратегию игры, подбирать под неё имеющиеся в его распоряжении элементы (персонажей и карты).

### Механики

Персонажи:
* Установка персонажа на выбранную клетку поля
* Перемещение персонажа на другую клетку
* Возврат персонажа в руку
* Уничтожение персонажа
* Воскрешение персонажа
* Изменение силы персонажа

Клетки поля:
* Открытие клетки поля
* Изменение эффекта клетки поля
* Исчезновение клетки поля
* Блокировка клетки поля

### Эмоциональные триггеры

* Learning
* Challenge

В будущем:
* Social interaction
* Acquisition

### Планы по дальнейшему развитию

* Реализовать разные уровни сложности
* Реализовать мультиплеер. Возможность организовывать матчи между двумя людьми.
* Сделать коллекцию карт. Наборы перед матчем можно составлять не из всех персонажей и карт, а лишь из тех, что есть в коллекции игрока.
* Придумать дополнительные режимы игры.

## Описание игры

### Подготовка к матчу
Когда игрок принимает решение начать игру, он попадает на экран подготовки к матчу.
На этом этапе он выбирает класс, за который будет играть.
Игрок получает 1000 монет, на которые он может приобрести персонажей и карты того класса, который он выбрал.
Монеты присутствуют только на этапе подготовки к матчу, после начала матча все оставшиеся монеты пропадают.

В игре присутствуют три класса, которые соответствуют трем факультетам КПИ: ФИВТ, ФПМ и ИПСА.
Каждый класс имеет свою стратегию игры, своих персонажей и карты.

Персонажи характеризуются одним параметром (помимо цены в монетах) – силой. 
Чем выше начальная сила персонажа – тем больше его начальное преимущество в бою.

Карты бывают трех типов: золотые, серебряные и нейтральные.
Золотая карта может использоваться только в бою, где не было использовано ни одной карты типа, отличного от нейтрального.
Серебряная карта – только в бою, где не было использовано ни одной золотой и не больше одной серебряной карты
(не больше двух серебряных карт за один бой).
Нейтральные карты могут быть использованы в любом количестве в любом бою.
Большая часть карт может играться только во время боя. Однако, есть карты, которые можно играть в любое время матча.
Такие карты имеют только нейтральный тип.

Краткая характеристика классов:
* **ИПСА.** Класс использует стратегию, ориентированную на победу силой.
Имеет сильных персонажей, а также карты, позволяющие без труда получить преимущество в силе.

* **ФИВТ.** Класс использует стратегию, ориентированную на игру большим количеством персонажей
и взаимодействие персонажей на поле. Имеет слабых, но дешевых персонажей и карты, позволяющие
получить дополнительные бонусы от взаимодействия персонажей на поле.

* **ФПМ.** Класс использует стратегию, ориентированную на игру большим количеством карт и взаимодействие с клетками поля.
Имеет дорогих персонажей и относительно дешевые карты, позволяющие в том числе взаимодействовать с полем игры.

### Матч
Игроки ходят по очереди. Ход игрока состоит в том, чтоб положить свою фишку на любую клетку поля, кроме тех, на которых
уже находятся союзные фишки. Также можно в свой ход сыграть нейтральную карту, если в её описании указано, что данная карта
может играться в любой момент.

Как только на клетке поля оказываются фишки обоих игроков, на этой клетке начинается бой. Клетка поля открывается, оказывая
эффект на силу персонажей в зависимости от их классовой принадлежности (бонус или штраф к силе).
Во время боя меняется структура хода игроков: теперь ход состоит не из выкладывания фишки на поле, а в разыгрывании карты.
Таким образом, во время боя игроки по очереди разыгрывают карты из руки. Имеют место ограничения на количество карт,
которые можно разыграть за один бой, в зависимости от типа карты. Они описаны выше, в абзаце с описанием всех типов карт.

Когда оба игрока в бою подряд пропускают ход, бой завершается. Фишка, имеющая больший уровень силы, побеждает и возвращается
в руку игрока. Проигравшая фишка идет в отбой. Если силы равны, обе фишки возвращаются к владельцам.

Когда один из игроков проигрывает все свои фишки, матч завершается. Побеждает игрок, у которого остались фишки на руках.