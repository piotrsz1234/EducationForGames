var countOfAnswers = 1;

function SetCountOfAnswers(count) {
	countOfAnswers = count;
	if (count == 0)
		count = 3;
}

function UpdateValues() {
	var array = new Array(countOfAnswers++);
	for (var i = 0; i < countOfAnswers-1; i++) {
		array[i] = document.getElementsByName("SeperatedAnswers["+i+"]").value;
	}
	var container = document.getElementById("");
	container.innerHTML = "";
	var form = '<input name="SeperatedAnswers[-1]" class="validate" id="questiontext" type="text" placeholder="Błędna odpowiedź" value="">' +
		'<label class="active" for="questiontext">Answer</label>';
	for (var i = 0; i < countOfAnswers; i++) {
		container.innerHTML += form.replace("-1", i);
	}
	for (var i = 0; i < array.length; i++) {
		document.getElementsByName("SeperatedAnswers["+i+"]").value = array[i];
	}
}

function Delete(value) {
	var xhttp = new XMLHttpRequest();
	xhttp.open("GET", "Question/Delete/"+value, true);
	xhttp.send();
}
