EXTERNAL Event(eventName)
EXTERNAL Get_Item(id)
EXTERNAL Add_Item(id, amount)
EXTERNAL Change_Hairstyle(id)

=== function Event(eventName)
// Fallback in case actual function is not available.
EVENT: {eventName}

=== function Get_Item(id)
GET_ITEM: {id}
~ return 0

=== function Add_Item(id, amount)
ADD_ITEM: {id} - VALUE: {amount}

=== function Change_Hairstyle(id)
CHANGE_HAIRSTYLE: {id}