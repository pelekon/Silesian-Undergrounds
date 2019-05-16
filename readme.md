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
| background.png  | Kamil Klyta  |
| bottom-left.png  | Kamil Klyta  |
| bottom-right.png  | Kamil Klyta  |
| bottom.png  | ?  |
| bottom-left.png  | ?  |
| box.png | ? |
| box_lit.png | ? |
| debug_box.png | ? |
| debug_circle.png | ? |
| default.png | ? |
| kolejne.png | ? |
| ledder.png | Kamil Klyta |
| left-entry-bottom.png | Kamil Klyta |
| left-entry-top.png | Kamil Klyta |
| left.png | ? |
| minerCharacter.png | ? |
| rays_map.png | Tomasz Kot |
| right-entry-bottom.png | Kamil Klyta |
| right-entry-top.png | Kamil Klyta |
| right.png | ? |
| test.png | ? |
| top-left.png | ? |
| top-right.png | ? |
| top.png | ? |
| x-blue.png | Tomasz Kot |
| x-green.png | Tomasz Kot |
| x.png | ? |
| zielone.png | ? |
  
### Spis użytych ścieżek dzwiękowych:
| Nazwa pliku w folderze Content  | Autor/Źródło |
| ------------- | ------------- |
| sample.mp3  | ?  |
| sample.mp3 | ?  |

