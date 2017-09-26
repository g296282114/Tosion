function gfun_height(bwidth, bheight) {
    $('#content').height(bheight);
    $('#gl_layout').layout().layout('resize');
    if ($("#gl_layout").layout('options').width < 400) {
        if (!$("#gl_layout").layout("panel", "east").panel('options').collapsed) {
            $("#gl_layout").layout("collapse", "east");
        }
    }
    else {
        if ($("#gl_layout").layout("panel", "east").panel('options').collapsed) {
            $("#gl_layout").layout("expand", "east");
        }
    }
}

function gfun_terminal_callback(data) {
    var vframes = $("iframe");
    for (var i = 0; i < vframes.length; i++) {
        if ($(vframes[i]).attr("src").toLowerCase().indexOf(data["tmodule"].toLowerCase()) >= 0) {

            var vframefun = vframes[i].contentWindow.gfun_terminal_callback;

            if (vframefun) {
                vframefun(data);
            }
        }
    }
    if (self.gfun_terminal_message)
        self.gfun_terminal_message(data);
}
