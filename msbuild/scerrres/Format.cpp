
#include "format.h"

VOID __stdcall FormatStarcounterErrorMessage(
	DWORD errorCode,
	LPWSTR outputBuffer, 
	DWORD outputBufferLength
)
{
	DWORD dr;
    int ir;

	// Since the length is the total length including the terminator character
    // we first allocate room for that. No matter if we run out of buffer space
    // or not we will now always have room for the terminator (assumes that the
    // legth of the buffer is at least 1).
    //  _ASSERT(length != 0);
    
	outputBufferLength--;
	dr = 0;

	// Load error message from the resource library into the buffer. If no
    // error message was found for the error code, or the buffer wasn't large
    // enough, we include a generic message instead.
    
	dr = FormatMessageW(
		FORMAT_MESSAGE_FROM_HMODULE,
		GetModuleHandleW(L"scerrres.dll"),
		errorCode,
		0,
		outputBuffer,
		outputBufferLength,
		NULL
	);
	if (dr == 0 && outputBufferLength >= 23)
    {
		ir = swprintf_s(outputBuffer, outputBufferLength, L"Error code=0x%X.", errorCode);
		dr = (DWORD)ir;
    }

	// Add a terminator and we are done
	
	outputBuffer += dr;
	*outputBuffer = 0;
}