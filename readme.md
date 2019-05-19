# Game Design Document

## Tytuł: Silesian undergrounds

## Autorzy:
- Bartłomiej Bukowiecki
- Kamil Klyta
- Tomasz Kot
- Bartłomiej Wloczyk

## High concept

Gracz wciela się w górnika imieniem Janusz. Janusz podczas codziennej, ciężkiej pracy w kopalni, wpadł w wyniku niefortunnego wypadku do głębokiej szczeliny i tak znalazł się w nieznanym podziemnym świecie. Gra należy do gatunku roguelike, a skupia się na przetrwaniu w nieznanym świecie i wydostaniu się z na powierzchnię. Głównym zadaniem gracza jest, poprzez pokonywanie kolejnych poziomów, wydostanie się na powierzchnię.

## Game flow

Podczas swojej wędrówki gracz będzie musiał walczyć z różnymi przeciwnikami oraz zbierać znajdźki i przedmioty, które będą zmieniały jego statystyki. Gra nastawiona jest na wielokrotne jej przechodzenie.

## Mechaniki:
- Generowana mapa, złożona z predefiniowanych jaskiń
- Pasek głodu, którego wartość zmniejsza się wraz z upływem czasu. Przy dojściu do 0 – gracz umiera
- Możliwość zbierania węgla oraz innych surowców służących jako waluta
- Sklep co poziom, możliwość zakupu jedzenia oraz ekwipunku
- Losowe generowanie znajdziek
- System walki, mechanika hack and slash
- Pokoje w kórych można znaleźć przedmioty specjalne

## Tilemapa i warstwy

- Warstwa 1 (Walls):
	bloki z którymi gracz koliduje

- Warstwa 2 (Background):
	bloki tła

- Warstwa 3 (Pickables):
	poprzez umieszczenie testury X (czerwony) z Tileset2 oznaczamy miejsca, w których mogą pojawić się przedmioty do podniesienia

- Warstwa 4 (Traps):
	poprzez umieszczenie testury X (zielony) z Tileset2 oznaczamy miejsca, w których mogą pojawić się płapki (kolce) zadające graczowi obrażenia

- Warstwa 5 (Transitions):
	służy do oznaczenia miejsca po wejściu na które użytkownik zostanie przeniesiony na następny poziom

- Warstwa 6 (Enemies):
	warstwa służąca do oznaczania miejsc, w których mogą pojawiać się przeciwnicy

- Warstwa 7 (SpecialItems):
	warstwa służąca do oznaczania miejsc, w których mogą pojawiać się przedmioty specjalne - mikstury zmieniające statystyki użytkownika

- Warstwa 8 (PlayerSpawn):
	warstwa służąca do oznaczania miejsca, w których spawnuje się gracz - celem uniknięcia ewentualnych kolizji z losowo
	generowanymi przedmiotami
- Warswta 9 (ShopPickables):
	warstwa służąca do generowania przedmiotów możliwych do zakupu przez użytkownika

- Warstwa 10 (RandomRooms):
	warstwa do oznaczania miejsc gdzie generują się losowe pokoje dla gracza


### Spis użytych grafik:

| Nazwa pliku w folderze Content  | Autor/Źródło |
| ------------- | ------------- |
| background.png  | [opengameart](https://opengameart.org/content/pixelart-menu-naturery-hand-shooting-some-green-stuff) |
| bottom-left.png  | Kamil Klyta  |
| bottom-right.png  | Kamil Klyta  |
| bottom.png  | Kamil Klyta |
| bottom-left.png  | Kamil Klyta |
| box.png | [opengameart](https://opengameart.org/content/pixelart-menu-naturery-hand-shooting-some-green-stuff) |
| box_lit.png | [opengameart](https://opengameart.org/content/pixelart-menu-naturery-hand-shooting-some-green-stuff) |
| debug_box.png | Bartosz Bukowiecki |
| debug_circle.png | Bartosz Bukowiecki |
| default.png | Bartosz Bukowiecki |
| kolejne.png | Bartłomiej Wloczyk |
| ledder.png | Kamil Klyta |
| left-entry-bottom.png | Kamil Klyta |
| left-entry-top.png | Kamil Klyta |
| left.png | Kamil Klyta |
| minerCharacter.png | [opengameart](https://opengameart.org/content/dwarves) |
| rays_map.png | Tomasz Kot |
| right-entry-bottom.png | Kamil Klyta |
| right-entry-top.png | Kamil Klyta |
| right.png | Kamil Klyta |
| test.png | Bartłomiej Bukowiecki |
| top-left.png | Kamil Klyta |
| top-right.png | Kamil Klyta |
| top.png | Kamil Klyta |
| x-blue.png | Tomasz Kot |
| x-green.png | Tomasz Kot |
| x.png | Bartłomiej Wloczyk |
| zielone.png | Bartłomiej Bukowiecki |
| chest_1.png | Kamil Klyta |
| chest_2.png | Kamil Klyta |
| chest_3.png | Kamil Klyta |
| chest_4.png | Kamil Klyta |
| meat_with_label.png | Tomasz Kot |
| meat.png | Kamil Klyta |
| steak.png | Kamil Klyta |
| grounds_[1..24].png | Kamil Klyta |
| wood_*.png | Kamil Klyta |
| heart.png | Kamil Klyta |
| heart_shop_1.png | Tomasz Kot |
| heart_1.png | Kamil Klyta |
| key_1.png | Kamil Klyta |
| key_2.png | Kamil Klyta |
| key_3.png | Kamil Klyta |
| key_4.png | Kamil Klyta |
| key_1_label.png | Tomasz Kot |
| coal.png | Kamil Klyta |
| gold_1.png | Kamil Klyta |
| gold_2.png | Kamil Klyta |
| gold_3.png | Kamil Klyta |
| silver_1.png | Kamil Klyta |
| silver_2.png | Kamil Klyta |
| silver_3.png | Kamil Klyta |
| atack-booster.png | Kamil Klyta |
| chest-drop-booster.png | Kamil Klyta |
| hunger-booster.png | Kamil Klyta |
| hunger-immunite-booster.png | Kamil Klyta |
| libe-booster.png | Kamil Klyta |
| movement-booster.png | Kamil Klyta |
| pickup-double-booster.png | Kamil Klyta |
| temporary_spike_1.png | Tomasz Kot |
| temporary_spike_2.png | [opengameart](https://opengameart.org/content/bevouliin-free-game-obstacle-spikes) |
| temporary_spike_3.png | [opengameart](https://opengameart.org/content/bevouliin-free-game-obstacle-spikes) |
| temporary_spike_4.png | [opengameart](https://opengameart.org/content/bevouliin-free-game-obstacle-spikes) |
| temporary_spike_5.png | [opengameart](https://opengameart.org/content/bevouliin-free-game-obstacle-spikes) |
| bricks-bottom.png | Kamil Klyta |
| bricks-ledder.png | Kamil Klyta |
| bricks-left.png | Kamil Klyta |
| bricks-left-bottom.png | Kamil Klyta |
| bricks-left.png | Kamil Klyta |
| bricks-left-transparent.png | Kamil Klyta |
| bricks-right.png | Kamil Klyta |
| bricks-right-bottom.png | Kamil Klyta |
| bricks-right.png | Kamil Klyta |
| bricks-top.png | Kamil Klyta |

### Spis użytych ścieżek dzwiękowych:
| Nazwa pliku w folderze Content  | Autor/Źródło |
| ------------- | ------------- |
| background_game.mp3  | [opengameart](https://opengameart.org/content/enchanted-tiki-86) |
| chest_opening.wav | [opengameart](https://opengameart.org/content/open-chest) |

