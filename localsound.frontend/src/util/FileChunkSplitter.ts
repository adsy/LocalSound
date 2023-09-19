import agent from "../api/agent";
import { v4 as uuidv4 } from "uuid";

export class FileChunkSplitter {
  uploadFile = (file: File, memberId: string) => {
    // create array to store the buffer chunks
    var FileChunk = [];
    // the file object itself that we will work with
    // set up other initial vars
    var MaxFileSizeMB = 1;
    var BufferChunkSize = MaxFileSizeMB * (1024 * 1024);
    var ReadBuffer_Size = 1024;
    var FileStreamPos = 0;
    // set the initial chunk length
    var EndPos = BufferChunkSize;
    var Size = file.size;

    var partialTrackId = uuidv4();

    // add to the FileChunk array until we get to the end of the file
    while (FileStreamPos < Size) {
      // "slice" the file from the starting position/offset, to  the required length
      FileChunk.push(file.slice(FileStreamPos, EndPos));
      FileStreamPos = EndPos; // jump by the amount read
      EndPos = FileStreamPos + BufferChunkSize; // set next chunk length
    }
    // get total number of "files" we will be sending
    var TotalParts = FileChunk.length;
    var PartCount = 0;
    console.log(FileChunk);
    // loop through, pulling the first item from the array each time and sending it
    for (let chunk; (chunk = FileChunk.shift()); ) {
      PartCount++;
      // file name convention
      var FilePartName = file.name + ".part_" + PartCount + "." + TotalParts;
      // send the file

      var formData = new FormData();
      formData.append("fileName", FilePartName);
      formData.append("formFile", chunk, FilePartName);
      agent.Tracks.uploadTrackChunk(
        memberId,
        partialTrackId,
        PartCount,
        formData
      );
    }
  };
}
