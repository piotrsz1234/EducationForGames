var countOfAnswers = 1;

function SetCountOfAnswers(count) {
	countOfAnswers = count;
	if (count == 0)
		count = 3;
}

function UpdateValues() {
	var array = new Array(countOfAnswers++);
	var value = document.getElementById("correct").value;
	for (var i = 0; i < countOfAnswers - 1; i++) {
		array[i] = document.getElementById("sa" + i).value;
	}
	var container = document.getElementById("answers");
	container.innerHTML = "";
	var form = '<div class="text-center">'+
		'<input type="text" class="d-inline-block" name="SeparatedAnswers[-1]" id="sa-1" placeholder="Answer" />'+
			'<div onclick="SetAnswer(-1)" id="answer-1" class="btn d-inline-block btn-link">Correct Answer</div></div>';
	for (var i = 0; i < countOfAnswers; i++) {
		if (i != value)
			container.innerHTML += form.replace("-1", i).replace("SetAnswer(-1)", "SetAnswer("+i+")").replace("sa-1", "sa" + i).replace("-1", i);
		else container.innerHTML += form.replace("-1", i).replace("SetAnswer(-1)", "SetAnswer(" + i + ")").replace("sa-1", "sa" + i).replace("btn-link", "btn-success").replace("-1", i);
	}
	for (var i = 0; i < array.length; i++) {
		document.getElementById("sa"+i).value = array[i];
	}
}

function Delete(value) {
	var xhttp = new XMLHttpRequest();
	xhttp.open("GET", "Question/Delete/"+value, true);
	xhttp.send();
}

function SetAnswer(value) {
	var t = document.getElementById("correct");
	var temp = document.getElementById("answer" + t.value);
	temp.className = temp.className.replace("btn-success", "btn-link");
	temp = document.getElementById("answer" + value);
	temp.className = temp.className.replace("btn-link", "btn-success");
	t.value = value;
}
