# Card Dealer

Hello, there are a few things I did not understand fully in the instruction:
1. Card Orientation: I don't know when we should display the card in Vertical or Horizontal orientation. But, as show in the [gif 004](#4), those orientation should be supported. 
2. Sorry I missed the positioning rules for cards 5 to 12. The grid utils I made only handles the positioning for 1-4. based on my understanding earlier, the cards should be distributed neatly at the center. [as shown in gif 005](#5)

A few things to note: 
- Due to my machine is running like a potato, I had to move my classes on its own assembly to reduce the compilation speed. I had to downgrade the projet to Unity 2021 too. Pardon security warnings. 
- A few of my custom package were used in the project, plus I have added TMP and Odin.
- The script files are located in Assets/CardDealer folder.

## Sample Videos are located in Output folders

## 1. Dynamic generation of dot grid based on a bounds
![img001a](Output/001-grid-bounds.gif)

# 2. 2 faced card flipping
![img001a](Output/002-card-flip.gif)

# 3. Demo of card distribution from the deck
![img001a](Output/003-deck-card-distribution.gif)

# 4. Showing that the orientation of cards can be supported anywhere from the target dots
![img001a](Output/004-card-orientation.gif)

# 5. Deal of cards 1 to 4
![img001a](Output/005-dealer.gif)
