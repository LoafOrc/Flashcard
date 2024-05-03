# Flashcard
For now make sure everyone has the same config. Networking has not been fully tested yet, from my own tests you are able to extract the footage fine.

Current Features:
- Change the max length of videos you can film with the camera.
  - By default Flashcard increases it from 90 (vanilla default) to 120 (2 minutes, flashcard default)
- Change the framerate filmed by the camera
  - Flashcard leaves this at the game's default of 24 fps.
- Change the bitrate filmed by the camera
  - This improves the quality of the final video
  - This setting also drastically changes filesize.
    - Flashcard's default sets it to 512kb, this about doubles the filesize from vanilla for a full 2 minute video (~4mb -> ~8mb)
- Config option to spawn in a second camera
  - I want to make this a more balanced game mechanic in the future where you'll actually need to purchase it, but for now its either you have 1 or 2 from the start.
- Config option to increase packet size when uploading the clip to friends. Should help with issues of failing to extract